namespace Shared.Dtos
{
    public class QuestionResultDto
    {
        public string Question { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!;
        public string StudentAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
