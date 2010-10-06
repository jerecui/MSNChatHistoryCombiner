using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsnHistoryCore
{
    public class MsnContext : SingletonBase<MsnContext>
    {
        public string TargetDirectoryPath { get; set; }
    }
}
