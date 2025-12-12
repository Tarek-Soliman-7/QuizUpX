namespace Shared.Dtos
{
    public record UpdateQuestionDto
    {
        public int? subjectId { get; set; }
        public string? title { get; set; }
        public List<string>? choices { get; set; }
        public int? correctIndex { get; set; }
        public int? mark { get; set; }
        public bool? questionType { get; set; }
    }
}
