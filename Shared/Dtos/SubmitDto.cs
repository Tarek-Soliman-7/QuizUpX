using System.Text.Json;

namespace Shared.Dtos
{
    public class SubmitDto
    {
        public int subjectId { get; set; }

        // اجابات الطالب: key = QuestionId, value = الإجابة (int أو bool أو string)
        public Dictionary<int, int> answers { get; set; } = new();

        // الوقت اللي استغرقه الطالب (اختياري)
        public int? timeTakenSeconds { get; set; }

        // الوقت اللي بدأ فيه الامتحان (اختياري)
        public DateTime? startedAt { get; set; }
    }
}
