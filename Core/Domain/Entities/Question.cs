namespace Domain.Entities
{
    public class Question
    {

        public int id { get; set; }

        public int subjectId { get; set; }


        public string title { get; set; } = string.Empty;

        public List<string> choices { get; set; } = new();

        public int correctIndex { get; set; }

        public int mark { get; set; } = 1;

        public bool questionType { get; set; } = false;

        public Subject? Subject { get; set; }

    }
}
