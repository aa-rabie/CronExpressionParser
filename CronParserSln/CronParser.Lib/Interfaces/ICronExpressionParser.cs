using CronParser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronParser.Lib.Interfaces
{
    public interface ICronExpressionParser
    {
        void Parse(CronExpression expr);
        void WriteToOutput();
    }
}
