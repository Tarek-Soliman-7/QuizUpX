using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AttemptAnswer
    {
        public int Id { get; set; }

        public int AttemptId { get; set; }
        public Attempt Attempt { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int SelectedAnswer { get; set; } // index or option number

        public bool IsCorrect { get; set; }
    }
}
