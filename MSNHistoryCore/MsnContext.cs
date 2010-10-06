
using System.Collections.Generic;
namespace MsnHistoryCore
{
    public class MsnContext : SingletonBase<MsnContext>
    {
        public string TargetDirectoryPath { get; set; }

        public List<string> SourceDirectories
        {
            get
            {
                return _sourceDirectories;
            }
        } private List<string> _sourceDirectories = new List<string>();
    }
}
