using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class StudentLoginRequestDto
    {
        public string StudentCode { get; set; } = null!;
        public string Pin { get; set; } = null!;
        public int SubjectId { get; set; }
    }
}
