using System;

namespace MsnHistoryCore
{
    public class InternalLogger
    {
        private static ILogger LogInstance;

        public static void InitializeLogger(ILogger logger)
        {
            LogInstance = logger;
        }

        public static void Write(string message)
        {
            if (LogInstance != null)
            {
                var formatedMessage = message;
                if (LogInstance.ShowTime)
                {
                    formatedMessage = string.Format("{0} {1} {2}",
                         DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), message);
                }

                LogInstance.Write(formatedMessage);
            }
        }

    }
}
