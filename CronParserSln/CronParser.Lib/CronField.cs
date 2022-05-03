using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace CronParser.Lib
{
    public class CronField
    {
        private readonly CronFieldType _type;
        private readonly int _min;
        private readonly int _max;

        /// <summary>
        /// TRUE if field input format is {small-number}-{large-number} , for ex: 1-5
        /// </summary>
        public bool IsRange { get; set; }
        /// <summary>
        /// TRUE if field input is *
        /// </summary>
        public bool IsAllValues { get; set; }

        /// <summary>
        /// TRUE if field input is */{num} , for example */15
        /// </summary>
        public bool IsInterval { get; set; }
        /// <summary>
        /// TRUE if field input is comma separated digits , for ex: 1, 3 , 7 
        /// </summary>
        public bool IsListOfValues { get; set; }

        private List<int> _values = new List<int>();

        const bool Success = true;

        public CronFieldType Type => _type;

        public List<int> Values => new List<int>(_values);

        public CronField(CronFieldType type, int min, int max)
        {
            if(min < 0)
                throw new ArgumentOutOfRangeException(nameof(min), "min value should be >= zero");

            if (max < min)
                throw new ArgumentOutOfRangeException(nameof(max), $"max value should be >= {min}");

            _type = type;
            _min = min;
            _max = max;
        }
        public  bool Parse(string input)
        {
            Reset();

            if(string.IsNullOrEmpty(input))
                throw new CronException($"Input is Null or Empty for field of type {GetEnumName(_type)}");

            input = input.Trim();

            ValidateThatInputHasSingleSymbolIfAny(input);

            try
            {
                //Check If Star
                if (CheckIfIsStar(input)) 
                    return Success;

                // Check If single digit
                if (CheckIfSingleNumericValue(input)) 
                    return Success;

                //Check If Range
                if (CheckIfRangeOfValues(input)) 
                    return Success;

                // Check If Interval
                if (CheckIfInterval(input)) 
                    return Success;

                // Check If comma separated list
                if (CheckIfCommaSeparatedListOfValues(input)) 
                    return Success;

                var errorMsg =
                        $"Invalid Field Input: ({input}) is invalid - field type : {GetEnumName(_type)}";

                throw new CronException(errorMsg);
            }
            catch (Exception e)
            {
                throw new CronException($"Exception thrown while parsing input '{input}' " , e);
            }
            
        }

        private bool CheckIfCommaSeparatedListOfValues(string input)
        {
            var commaIndex = input.IndexOf(',');
            if (commaIndex > 0)
            {
                var vals = input.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries
                                                    | StringSplitOptions.TrimEntries);

                if (!vals.Any())
                {
                    var errorMsg =
                        $"Invalid Expression: Input '{input}' format is invalid - field type : {GetEnumName(_type)}";

                    throw new CronException(errorMsg);
                }

                foreach (var strVal in vals)
                {
                    var iVal = int.Parse(strVal);
                    ValidateValueWithinRange(iVal, input);
                    _values.Add(iVal);
                }

                IsListOfValues = true;

                return true;
            }

            return false;
        }

        private bool CheckIfInterval(string input)
        {
            var slashIndex = input.IndexOf('/');

            if (slashIndex > 0)
            {
                var interval = int.Parse(input.Substring(slashIndex + 1), CultureInfo.InvariantCulture);
                ValidateValueWithinRange(interval, input);
                // add values to _values
                AddIntervalToValues(interval);
                IsInterval = true;

                return true;
            }

            return false;
        }

        private bool CheckIfRangeOfValues(string input)
        {
            var dashIndex = input.IndexOf('-');

            if (dashIndex > 0)
            {
                var first = int.Parse(input.Substring(0, dashIndex), CultureInfo.InvariantCulture);
                ValidateValueWithinRange(first, input);
                var last = int.Parse(input.Substring(dashIndex + 1), CultureInfo.InvariantCulture);
                ValidateValueWithinRange(last, input);

                if (last <= first)
                {
                    throw new CronException($"Invalid Range Format in '{input}': {first} should be < {last}");
                }

                for (var valInRange = first; valInRange <= last; valInRange++)
                {
                    _values.Add(valInRange);
                }

                IsRange = true;
                return true;
            }

            return false;
        }

        private bool CheckIfSingleNumericValue(string input)
        {
            if (input.Any(ch => !char.IsNumber(ch))) 
                return false;

            var val = int.Parse(input, CultureInfo.InvariantCulture);
            ValidateValueWithinRange(val, input);
            _values.Add(val);
            return true;

        }

        private bool CheckIfIsStar(string input)
        {
            if (input.Length == 1 && input[0] == '*')
            {
                _values.AddRange(Enumerable.Range(_min, _max - _min + 1));
                IsAllValues = true;
                return true;
            }

            return false;
        }

        private void Reset()
        {
            _values = new List<int>();
            IsInterval = false;
            IsListOfValues = false;
            IsRange = false;
            IsAllValues = false;
        }

        private void AddIntervalToValues(int interval)
        {
            for (int step = _min ; step <= _max; step+= interval)
            {
                _values.Add(step);
            }
        }

        private void ValidateThatInputHasSingleSymbolIfAny(string input)
        {
            input = input.Replace(" ", string.Empty);
            if (input.StartsWith("*/"))
            {
                input = input.Substring(1);
            }
            var symbols = new[] {'*', '/', '-'};
            var symbolsFound = 0;
            foreach (var s in symbols)
            {
                if (input.Contains(s))
                {
                    symbolsFound += 1;
                }
            }

            var errorMsg =
                $"Invalid Expression: Input '{input}' has multiple special characters - field type : {GetEnumName(_type)}";

            if (symbolsFound > 1)
            {
                throw new CronException(errorMsg);
            }

            if (input.Contains(',') && symbolsFound > 0)
            {
                throw new CronException(errorMsg);
            }
        }

        private string GetEnumName(CronFieldType type)
        {
            return Enum.GetName(typeof(CronFieldType), type);
        }

        private void ValidateValueWithinRange(int val, string expr)
        {
            if(val < _min)
                throw new CronException($"Invalid value: value {val} should be >= {_min} - input expression : '{expr}' - field type : {GetEnumName(_type)}");

            if (val > _max)
                throw new CronException($"Invalid value: value {val} should be <= {_max} - input expression : '{expr}' - field type : {GetEnumName(_type)}");
        }
    }
}
