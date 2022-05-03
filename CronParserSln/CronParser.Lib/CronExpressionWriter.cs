using CronParser.Lib.Interfaces;
using CronParser.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CronParser.Lib
{
    public class CronExpressionWriter : ICronExpressionWriter
    {
        private readonly IOutputWriter _writer;
        private const int _fieldNameColumnWidth = 14;
        private const int _distanceBetweenColumn = 5;
        private readonly string _commandFieldLabel = "command";

        public CronExpressionWriter(IOutputWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public void WriteFormatted(CronExpression expr, List<CronField> fields)
        {
            if(expr == null)
                throw new ArgumentNullException(nameof(expr));

            if (fields == null || !fields.Any())
                throw new ArgumentNullException(nameof(fields) ,"fields collection is null or empty");

            WriteField(fields.First(f => f.Type == CronFieldType.Minute));
            WriteField(fields.First(f => f.Type == CronFieldType.Hour));
            WriteField(fields.First(f => f.Type == CronFieldType.DayOfMonth));
            WriteField(fields.First(f => f.Type == CronFieldType.Month));
            WriteField(fields.First(f => f.Type == CronFieldType.DayOfWeek));

            WriteCommand(expr);
        }

        private void WriteCommand(CronExpression expr)
        {
            WriteFieldNameColumn(_commandFieldLabel);
            WriteDistanceBetweenColumns();
            _writer.Write(expr.Command);

            _writer.Write($"{Environment.NewLine}");
        }

        private void WriteField(CronField field)
        {
            var fieldName = GetFieldLabel(field.Type);
            WriteFieldNameColumn(fieldName);
            WriteDistanceBetweenColumns();
            WriteValueColumn(field.Values);

            _writer.Write($"{Environment.NewLine}");
        }

        private string GetFieldLabel(CronFieldType type)
        {
            switch (type)
            {
                case CronFieldType.Minute:
                    return "minute";
                case CronFieldType.Hour:
                    return "hour";
                case CronFieldType.DayOfMonth:
                    return "day of month";
                case CronFieldType.Month:
                    return "month";
                case CronFieldType.DayOfWeek:
                    return "day of week";
            }

            return string.Empty;
        }

        private void WriteFieldNameColumn(string val)
        {
            _writer.Write(val.PadRight(_fieldNameColumnWidth, ' '));
        }

        private void WriteDistanceBetweenColumns()
        {
            _writer.Write(string.Concat(Enumerable.Repeat(" ", _distanceBetweenColumn)));
        }

        private void WriteValueColumn(List<int> values)
        {
            _writer.Write(string.Join<int>(' ', values));
        }
    }
}
