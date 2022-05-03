using System;

namespace CronParser.Lib
{
    public class CronException : Exception
    {
        public CronException(string message) :
            base(message)
        { }

        public CronException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}