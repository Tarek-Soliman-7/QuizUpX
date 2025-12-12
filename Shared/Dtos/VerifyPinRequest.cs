using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class VerifyPinRequest
    {
        public string UniversityCode { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
    }
}
