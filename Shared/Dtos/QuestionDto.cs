namespace Shared.Dtos
{
    public class QuestionDto
    {
        public int questionId { get; set; }
        public string title { get; set; } = null!;
        public List<string> choices { get; set; } = new();
        public int correctIndex { get; set; }
        public int mark { get; set; }
    }
}
