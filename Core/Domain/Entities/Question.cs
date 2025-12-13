namespace Domain.Entities
{
    public class Question
    {

        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public List<string> Choices { get; set; } = new();
        public int CorrectIndex { get; set; }
        public int Mark { get; set; }
        public bool QuestionType {  get; set; }
        public Subject Subject { get; set; } = null!;
        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    }
}
