using Domain.Entities.IdentityModule;
using Shared.Enums;
using System.Text.Json;

namespace Domain.Entities
{
    public class Attempt
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }

        public int TotalScore { get; set; }
        public int CorrectAnswers { get; set; }
        public bool Status { get; set; }   
        public Student Student { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public ICollection<AttemptAnswer> Answers { get; set; } = new List<AttemptAnswer>();

    }

}
