using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Option
    {
        public Guid Id { get; set; } 

        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = new Question();

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }=string.Empty;

        // Mark the correct option (for MCQ)
        public bool IsCorrect { get; set; } 

    }
}
