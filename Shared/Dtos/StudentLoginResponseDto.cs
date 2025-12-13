using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class StudentLoginResponseDto
    {
        public bool Success { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string Message { get; set; } = null!;
    }

}
