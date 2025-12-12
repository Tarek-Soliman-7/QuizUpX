namespace Shared.Dtos
{
    public class QuestionResultDto
    {
        public int id { get; set; }
        public bool isCorrect { get; set; }
        public int mark { get; set; }
        public int correctIndex { get; set; } // include only in review endpoints
    }
}
