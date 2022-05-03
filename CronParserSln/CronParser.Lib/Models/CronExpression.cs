using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronParser.Lib.Models
{
    public class CronExpression
    {
        public string Minute { get; set; }
        public string Hour { get; set;}
        public string DayOfMonth { get; set; }
        public string Month { get; set; }
        public string DayOfWeek { get; set; }
        public string Command { get; set; }
    }
}
