using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public record SubjectDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;

    }
}
