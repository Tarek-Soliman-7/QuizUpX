using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class SubmitQuizDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }

}
