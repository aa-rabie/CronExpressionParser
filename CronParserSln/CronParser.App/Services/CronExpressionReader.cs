using CronParser.Lib.Models;
using System;
using System.Linq;

namespace CronParser.App.Services
{
    internal class CronExpressionReader
    {
        internal static CronExpression Read(string[] args)
        {
            if(args == null || args.Length < 6)
            {
                var errMsg = "Required arguments are missing. expected number of arguments is 6 ";
                if(args != null)
                {
                    errMsg = $"{errMsg} , # of input args : {args.Length}";
                    if (args.Any())
                    {
                        errMsg = $"{errMsg} , input args are : {string.Join(' ',args)}";
                    }
                }
                throw new ArgumentException(errMsg);
            }

            return new CronExpression
            {
                Minute = args[0],
                Hour = args[1],
                DayOfMonth = args[2],
                Month = args[3],
                DayOfWeek = args[4],
                Command = args[5],
            };
        }
    }
}
