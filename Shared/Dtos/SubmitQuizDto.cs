using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class SubmitQuizDto
    {
        public int studentId { get; set; }
        public int subjectId { get; set; }
        public List<SubmitAnswerDto> answers { get; set; } = new();
    }

}
