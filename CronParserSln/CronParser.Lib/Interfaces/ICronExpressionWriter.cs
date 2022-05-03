using CronParser.Lib.Models;
using System.Collections.Generic;

namespace CronParser.Lib.Interfaces
{
    public interface ICronExpressionWriter
    {
        void WriteFormatted(CronExpression expr, List<CronField> fields);
    }
}
