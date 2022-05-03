using CronParser.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronParser.App.Services
{
    public class OutputWriter : IOutputWriter
    {
        public void Write(string val)
        {
            if (string.IsNullOrEmpty(val))
                return;

            Console.Write(val);
        }

        public void WriteLine(string line)
        {
            if(string.IsNullOrEmpty(line))
                return;

            Console.WriteLine(line);
        }
    }
}
