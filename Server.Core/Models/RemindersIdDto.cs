using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.Server.Core.Models
{
    public class RemindersIdDto
    {
        public const int LimitMaxValue = 50;

        public List<long> Id { get; set; }
    }
}
