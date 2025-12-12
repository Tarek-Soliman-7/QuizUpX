using Domain.Entities.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IStudentService
    {
        Task IssuePinAsync(string universityCode, string? plainPin = null, int? validMinutes = null);
        Task ResetPinAsync(string universityCode, string newPin);
        Task<bool> VerifyPinAsync(string universityCode, string providedPin);
        Task<bool> IsLockedOutAsync(string universityCode);
        Task<Student?> GetByUniversityCodeAsync(string universityCode);
        Task<Student> CreateStudentAsync(string universityCode, string fullName, string pin);
    }
}
