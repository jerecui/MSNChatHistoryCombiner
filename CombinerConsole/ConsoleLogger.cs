using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsnHistoryCore;

namespace CombinerConsole
{
    public class ConsoleLogger:SingletonBase<ConsoleLogger>, ILogger
    {
        public bool ShowTime
        {
            get { return false; }
        }

        public void Write(string text)
        {
            Console.WriteLine(text);
        }
    }
}
