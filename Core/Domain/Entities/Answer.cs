using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Answer
    {
        public Guid Id { get; set; } 

        [Required]
        public Guid AttemptId { get; set; }
        public Attempt Attempt { get; set; } = new Attempt();

        [Required]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }= new Question();

        // For MCQ
        public Guid? SelectedOptionId { get; set; }
        public Option SelectedOption { get; set; } = new Option();

        // For TF
        public bool? AnswerBool { get; set; }

        // Evaluated at submit time
        public bool? IsCorrect { get; set; }

        
    }
}
