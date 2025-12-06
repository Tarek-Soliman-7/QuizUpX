using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; } 

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }=string.Empty;

        // Owner
        [Required]
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; } = new User();

        // Business rules
        public int NumQuestions { get; set; } 
        public int DurationMinutes { get; set; } 

        public bool IsPublished { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime? PublishedAt { get; set; }

        // Navigation
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
    }
}
