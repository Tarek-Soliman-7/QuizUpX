namespace Shared.Dtos
{
    public class SubmitResultDto
    {
        public int AttemptId { get; set; }          // if saved
        public int TotalQuestions { get; set; }
        public int CorrectCount { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public Dictionary<int, int> answers { get; set; } = new();

        public List<QuestionResultDto> Details { get; set; } = new();
    }
}
