using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; } 

        [Required]
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = new Quiz();

        [Required]
        public QuestionType Type { get; set; } 

        [Required]
        public string Text { get; set; }=string.Empty;

        // For TF questions: store correct boolean (null for MCQ)
        public bool? CorrectAnswerBool { get; set; }

        // Navigation
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
