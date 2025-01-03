using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestro.Server.Core.Models
{
    public class RemindersForUserDtoWithTimeRange : RemindersForUserDto
    {
        public DateTime InclusiveStartDate { get; set; }
        public DateTime ExclusiveEndDate { get; set; }
    }
}
