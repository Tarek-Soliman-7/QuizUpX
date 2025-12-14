using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class StudentLoginRequestDto
    {
        public string universityCode { get; set; } = null!;
        public string pin { get; set; } = null!;
        public int subjectId { get; set; }
    }
}
