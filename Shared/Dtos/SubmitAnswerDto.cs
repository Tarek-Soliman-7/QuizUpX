using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class SubmitAnswerDto
    {
        public int questionId { get; set; }
        public int selectedIndex { get; set; }
    }
}
