namespace Shared.Dtos
{
    public class StartQuizRequest
    {
        public int SubjectId { get; set; }
        public string UniversityCode { get; set; } = null!;
        public string? Pin { get; set; } // if required by your StudentService
        public int? Take { get; set; } // optional number of questions
    }
}
