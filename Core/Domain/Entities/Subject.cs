namespace Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = string.Empty;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    }
}
