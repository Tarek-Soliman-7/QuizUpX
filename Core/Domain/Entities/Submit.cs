namespace Domain.Entities
{
    public class Submit
    {
        public int subjectId { get; set; }
        public Dictionary<int, object> answers { get; set; } = new();
    }
}
