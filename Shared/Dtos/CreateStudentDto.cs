using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class CreateStudentDto
    {
        public string UniversityCode { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Pin { get; set; } = null!;
    }
}
