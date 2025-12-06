using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Attempt
    {
        public Guid Id { get; set; } 

        [Required]
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }=new Quiz();

        [Required]
        public Guid StudentId { get; set; }
        public User Student { get; set; } = new User();

        [Required]
        public DateTime StartedAt { get; set; } 

        public DateTime? FinishedAt { get; set; }
        public bool IsSubmitted { get; set; } 

        // Score as percent 0.00 - 100.00
        public int? Score { get; set; }


        // Navigation
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    }
}
