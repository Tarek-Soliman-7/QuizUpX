using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class QuestionSeedModel
    {
        public string SubjectName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public List<string> Choices { get; set; } = new();
        public int CorrectIndex { get; set; }
        public int Mark { get; set; }
    }

}
