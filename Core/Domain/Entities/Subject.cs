namespace Domain.Entities
{
    public class Subject
    {
        public int id { get; set; }
        public string name { get; set; }=string.Empty;
        public string? description { get; set; } = string.Empty;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    }
}
