using System.Text.Json;

namespace Domain.Entities
{
    public class Attempt
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string? UserId { get; set; }           // optional if anonymous
        public DateTime StartedAt { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectCount { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public int TimeTakenSeconds { get; set; }
        public string AnswersJson { get; set; } = JsonSerializer.Serialize(new Dictionary<int, int>()); // JSON: { "<questionId>": <answer> }
        public string? IpAddress { get; set; }
        public string? DeviceInfo { get; set; }
        public string Status { get; set; } = "Submitted";
        public string UniversityCode { get; set; } = null!;

        // navigation
        public Subject? Subject { get; set; }
        // public ApplicationUser? User { get; set; } // if identity
    }

}
