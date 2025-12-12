namespace Shared.Dtos
{
    public class StartQuizResponse
    {
        public int AttemptId { get; set; }
        public int SubjectId { get; set; }
        public DateTime StartedAt { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
        public int? TimeLimitSeconds { get; set; }
    }
}
