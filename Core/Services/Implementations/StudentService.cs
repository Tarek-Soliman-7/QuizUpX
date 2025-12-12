using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts;
using System;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher<string> _hasher;
        private readonly ILogger<StudentService> _logger;

        // config
        private const int MAX_FAILED = 5;
        private static readonly TimeSpan LOCKOUT_TIME = TimeSpan.FromMinutes(15);

        public StudentService(
            IStudentRepository repo,
            IUnitOfWork uow,
            IPasswordHasher<string> hasher,
            ILogger<StudentService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Issue a PIN for a student. If plainPin is null the service will generate one.
        /// Stores the PIN as a hashed value (PinHash) and optionally keeps the plain PIN in Student.Pin
        /// only if you need it for immediate delivery (recommendation: DO NOT keep plain PIN in production).
        /// </summary>
        public async Task IssuePinAsync(string universityCode, string? plainPin = null, int? validMinutes = null)
        {
            if (string.IsNullOrWhiteSpace(universityCode))
                throw new ArgumentException("universityCode is required", nameof(universityCode));

            universityCode = universityCode.Trim();
            var student = await _repo.GetByUniversityCodeAsync(universityCode);

            var pinToUse = string.IsNullOrWhiteSpace(plainPin) ? GenerateNumericPin(6) : plainPin!.Trim();
            var pinHash = _hasher.HashPassword(universityCode, pinToUse);

            if (student == null)
            {
                student = new Student
                {
                    UniversityCode = universityCode,
                    FullName = null,
                    PinHash = pinHash,
                    // For development you may store the plain PIN temporarily; in production prefer to send it and not store.
                    Pin = null, // do not store plain pin by default
                    IsActive = true,
                    FailedAttempts = 0,
                    CreatedAt = DateTime.UtcNow,
                    LastPinResetAt = DateTime.UtcNow
                };

                await _repo.AddAsync(student);
            }
            else
            {
                student.PinHash = pinHash;
                // optional: keep plain for immediate admin response; set to null for security
                student.Pin = null;
                student.IsActive = true;
                student.FailedAttempts = 0;
                student.LockoutEnd = null;
                student.LastPinResetAt = DateTime.UtcNow;

                await _repo.UpdateAsync(student);
            }

            await _uow.CommitAsync();

            // NOTE: do NOT return the plain PIN in production. Send it via email/SMS.
            _logger.LogInformation("Issued PIN for {code}. (plain pin not logged)", universityCode);
        }

        /// <summary>
        /// Reset pin for the given student (admin action). Stores hashed pin.
        /// </summary>
        public async Task ResetPinAsync(string universityCode, string newPin)
        {
            if (string.IsNullOrWhiteSpace(universityCode)) throw new ArgumentException(nameof(universityCode));
            if (string.IsNullOrWhiteSpace(newPin)) throw new ArgumentException(nameof(newPin));

            universityCode = universityCode.Trim();
            var student = await _repo.GetByUniversityCodeAsync(universityCode);
            if (student == null) throw new KeyNotFoundException($"Student {universityCode} not found.");

            student.PinHash = _hasher.HashPassword(universityCode, newPin.Trim());
            student.Pin = null; // clear plain
            student.FailedAttempts = 0;
            student.LockoutEnd = null;
            student.LastPinResetAt = DateTime.UtcNow;

            await _repo.UpdateAsync(student);
            await _uow.CommitAsync();

            _logger.LogInformation("Reset PIN for {code}", universityCode);
        }

        /// <summary>
        /// Verify a provided PIN for a student. Supports hybrid mode:
        /// - if PinHash exists -> verify hashed
        /// - else if legacy Pin exists -> compare plain and migrate to hashed on success
        /// </summary>
        public async Task<bool> VerifyPinAsync(string universityCode, string providedPin)
        {
            if (string.IsNullOrWhiteSpace(universityCode) || string.IsNullOrWhiteSpace(providedPin))
                return false;

            universityCode = universityCode.Trim();
            providedPin = providedPin.Trim();

            var student = await _repo.GetByUniversityCodeAsync(universityCode);
            if (student == null || !student.IsActive) return false;

            // check lockout
            if (student.LockoutEnd.HasValue && student.LockoutEnd.Value > DateTime.UtcNow)
            {
                _logger.LogWarning("Student {code} is locked out until {lockout}", universityCode, student.LockoutEnd);
                return false;
            }

            // Case A: hashed pin exists -> verify
            if (!string.IsNullOrEmpty(student.PinHash))
            {
                var res = _hasher.VerifyHashedPassword(universityCode, student.PinHash, providedPin);
                if (res == PasswordVerificationResult.Success)
                {
                    // reset counters on success
                    student.FailedAttempts = 0;
                    student.LockoutEnd = null;
                    await _repo.UpdateAsync(student);
                    await _uow.CommitAsync();
                    return true;
                }

                // failed attempt
                student.FailedAttempts++;
                if (student.FailedAttempts >= MAX_FAILED)
                    student.LockoutEnd = DateTime.UtcNow.Add(LOCKOUT_TIME);

                await _repo.UpdateAsync(student);
                await _uow.CommitAsync();
                return false;
            }

            // Case B: legacy plain stored in student.Pin (or QuizAppCode if you used that name)
            if (!string.IsNullOrEmpty(student.Pin))
            {
                if (string.Equals(student.Pin.Trim(), providedPin, StringComparison.Ordinal))
                {
                    // migrate to hashed pin and clear legacy plain value
                    student.PinHash = _hasher.HashPassword(universityCode, providedPin);
                    student.Pin = null; // clear plain for security

                    student.FailedAttempts = 0;
                    student.LockoutEnd = null;
                    student.LastPinResetAt = DateTime.UtcNow;

                    await _repo.UpdateAsync(student);
                    await _uow.CommitAsync();
                    return true;
                }
                else
                {
                    // failed attempt on legacy plain
                    student.FailedAttempts++;
                    if (student.FailedAttempts >= MAX_FAILED)
                        student.LockoutEnd = DateTime.UtcNow.Add(LOCKOUT_TIME);

                    await _repo.UpdateAsync(student);
                    await _uow.CommitAsync();
                    return false;
                }
            }

            // no pin info found
            return false;
        }

        public async Task<bool> IsLockedOutAsync(string universityCode)
        {
            if (string.IsNullOrWhiteSpace(universityCode)) return false;
            var student = await _repo.GetByUniversityCodeAsync(universityCode.Trim());
            if (student == null) return false;
            return student.LockoutEnd.HasValue && student.LockoutEnd.Value > DateTime.UtcNow;
        }

        public async Task<Student?> GetByUniversityCodeAsync(string universityCode)
        {
            if (string.IsNullOrWhiteSpace(universityCode)) return null;
            return await _repo.GetByUniversityCodeAsync(universityCode.Trim());
        }

        public async Task<Student> CreateStudentAsync(string universityCode, string fullName, string pin)
        {
            // Check duplicate
            if (string.IsNullOrWhiteSpace(universityCode)) throw new ArgumentException(nameof(universityCode));
            universityCode = universityCode.Trim();

            var exists = await _repo.GetByUniversityCodeAsync(universityCode);
            if (exists != null)
                throw new InvalidOperationException("Student already exists.");

            var student = new Student
            {
                UniversityCode = universityCode,
                FullName = fullName,
                
                // store hashed pin, do NOT keep plain pin
                PinHash = _hasher.HashPassword(universityCode, pin),
                Pin = pin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastPinResetAt = DateTime.UtcNow
            };

            await _repo.AddAsync(student);
            await _uow.CommitAsync();

            return student;
        }

        // helper: generate numeric pin
        private static string GenerateNumericPin(int length = 6)
        {
            var rng = new Random();
            var sb = new System.Text.StringBuilder(length);
            for (int i = 0; i < length; i++) sb.Append(rng.Next(0, 10));
            return sb.ToString();
        }
    }
}
