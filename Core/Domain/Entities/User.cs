using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{

    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }=string.Empty;
        [MaxLength(14)]
        public string UniversityCode { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Password {  get; set; } = string.Empty;
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>(); // For Doctor
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>(); // For Student

    }
}
