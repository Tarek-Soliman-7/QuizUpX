namespace Shared.Dtos
{
    public class AttemptDto
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
    }
}
