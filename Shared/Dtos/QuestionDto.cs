namespace Shared.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public List<string> Choices { get; set; } = new();
        public int CorrectIndex { get; set; }
        public int Mark { get; set; }
    }
}
