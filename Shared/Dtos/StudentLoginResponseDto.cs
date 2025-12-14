using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class StudentLoginResponseDto
    {
        public bool success { get; set; }
        public int? studentId { get; set; }
        public string? studentName { get; set; }
        public string? message { get; set; } = null!;
    }

}
