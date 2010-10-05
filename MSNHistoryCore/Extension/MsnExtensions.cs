using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsnHistoryCore
{
    public static class MsnExtensions
    {
        public static string ToMsnUniversalString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK");
        }
    }
}
