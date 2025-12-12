using System.Text.Json;

namespace Shared.Dtos
{
    public class AttemptDetailsDto : AttemptDto
    {
        public string AnswersJson { get; set; }=JsonSerializer.Serialize(new Dictionary<int, int>());
        public List<QuestionResultDto> Details { get; set; } = new();
    }
}
