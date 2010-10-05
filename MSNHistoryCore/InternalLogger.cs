using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsnHistoryCore
{
    public class InternalLogger
    {
        public static ILogger LogInstance;

        public static void InitializeLogger(ILogger logger)
        {
            LogInstance = logger;
        }

        public static void Write(string message)
        {
            if (LogInstance != null)
            {
                var formatedMessage = string.Format("{0} {1} {2}",
                  DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), message);
                LogInstance.Write(formatedMessage);
            }
        }

    }
}
