using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos
{
    public record CreateSubjectDto
    {
        [Required]
        [StringLength(200)]
        public string name { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
    }
}
