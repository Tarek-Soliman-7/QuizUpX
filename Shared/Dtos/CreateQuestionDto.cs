using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos
{
    public record CreateQuestionDto
    {
        [Required]
        public int subjectId { get; set; }

        [Required]
        [StringLength(1000)]
        public string title { get; set; } = string.Empty;

        public List<string> choices { get; set; } = new();

        [Required]
        public int correctIndex { get; set; }

        public int mark { get; set; } = 1;

        public bool questionType { get; set; } = false;
    }
}
