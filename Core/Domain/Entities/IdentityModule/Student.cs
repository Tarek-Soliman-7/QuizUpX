using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.IdentityModule
{
    public class Student
    {

        public int Id { get; set; }   // Primary Key (Identity)

        public string UniversityCode { get; set; } = null!;
        public string? FullName { get; set; }

        // 🔐 NEW: Hashed PIN (recommended)
        public string? PinHash { get; set; }
        public string? Pin { get; set; }


        // 🔐 Legacy field (optional) if you previously stored plain text PIN

        // 🔐 Security-related
        public int FailedAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        // Activation flag (optional but useful)
        public bool IsActive { get; set; } = true;

        // Tracking fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastPinResetAt { get; set; }
    }
}
