namespace Shared.Dtos
{
    public record UpdateQuestionDto
    {
        public int? SubjectId { get; set; }
        public string? Title { get; set; }
        public List<string>? Choices { get; set; }
        public int? CorrectIndex { get; set; }
        public int? Mark { get; set; }
        public bool? QuestionType { get; set; }
    }
}
