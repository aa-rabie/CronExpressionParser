using CronParser.Lib.Interfaces;
using CronParser.Lib.Models;
using System.Collections.Generic;

namespace CronParser.Lib
{
    public class CronExpressionParser : ICronExpressionParser
    {
        //TODO : after validating each field , we need to validate that day of month value match month value
        // question how to validate if user specified multiple multiple values
        // possible solution => this validation can moved to the scheduler component when we need to generate 
        // list of execution date/time(s)

        private readonly ICronExpressionWriter _writer;

        private CronField _minute = new CronField(CronFieldType.Minute, 0, 59);
        private CronField _hour = new CronField(CronFieldType.Hour, 0, 23);
        private CronField _dayOfMonth = new CronField(CronFieldType.DayOfMonth, 1, 31);
        private CronField _month = new CronField(CronFieldType.Month, 1, 12);
        private CronField _dayOfWeek = new CronField(CronFieldType.DayOfWeek, 0, 6);

        private CronExpression _expression = default;

        internal List<CronField> Fields => new List<CronField> { _minute, _hour, _dayOfMonth, _month, _dayOfWeek };

        public CronExpressionParser(ICronExpressionWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public void Parse(CronExpression expr)
        {
            if (expr == null)
                throw new System.ArgumentNullException(nameof(expr));

            _minute.Parse(expr.Minute);
            _hour.Parse(expr.Hour);
            _dayOfMonth.Parse(expr.DayOfMonth);
            _month.Parse(expr.Month);
            _dayOfWeek.Parse(expr.DayOfWeek);

            _expression = expr;

        }

        public void WriteToOutput()
        {
            if (_expression == null)
                return;

            _writer.WriteFormatted(_expression, Fields);
        }
    }
}