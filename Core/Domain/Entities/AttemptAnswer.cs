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
        public int QuestionId { get; set; }
        public int SelectedIndex { get; set; }
        public bool IsCorrect { get; set; }
        
        public Attempt Attempt { get; set; } = null!;
        public Question Question { get; set; } = null!;
    }
}
