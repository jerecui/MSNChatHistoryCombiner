using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsnHistoryCore;
using System.IO;

namespace CombinerConsole
{
    class Program
    {
        //string pattern = "-(?<k>\\w)\\s+(?<p>(\"[^\"]*\")|[^\\s]*)";
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelpMessage();
                return;
            } 

            if (args.Length % 2 != 0)
            {
                var firstCmd = args[0].Replace("-", string.Empty).Replace("/", string.Empty);
                //if (string.Compare(firstCmd, "h", StringComparison.OrdinalIgnoreCase) != 0
                //    && string.Compare(firstCmd, "?", StringComparison.OrdinalIgnoreCase) != 0)
                    ShowHelpMessage();

                return;
            }

            for (int i = 0; (i * 2 + 1) < args.Length; i++)
            {
                var cmd = args[2 * i];
                var cmdValue = args[2 * i + 1];

                cmd = cmd.Replace("/", "-");
                cmdValue = cmdValue.Replace("\"", string.Empty);

                switch (cmd)
                {
                    case "-s":
                        var souces = cmdValue.Split(',');
                        MsnContext.Instance.SourceDirectories.AddRange(souces);
                        break;
                    case "-t":
                        MsnContext.Instance.TargetDirectoryPath = cmdValue;
                        break;
                    default:
                        ShowHelpMessage();
                        break;
                }
            }

            if (CheckParam() == false) return;

            InternalLogger.InitializeLogger(ConsoleLogger.Instance);

            MsnHistoryCombineWorker.Instance.Scan();
            MsnHistoryCombineWorker.Instance.Merge();
        }

        private static bool CheckParam()
        {
            var validParam = true;
            try
            {
                foreach (var souceDirectory in MsnContext.Instance.SourceDirectories)
                {
                    validParam &= CheckDirectory(souceDirectory);
                }

                if (string.IsNullOrEmpty(MsnContext.Instance.TargetDirectoryPath))
                {
                    validParam = false;
                    Console.WriteLine("No target directory path input.");
                }
                else
                {
                    var existTarget = CheckDirectory(MsnContext.Instance.TargetDirectoryPath);
                    if (existTarget == false) Directory.CreateDirectory(MsnContext.Instance.TargetDirectoryPath);
                    validParam &= CheckDirectory(MsnContext.Instance.TargetDirectoryPath);
                }
            }
            finally
            {
            }
            return validParam;
        }

        private static bool CheckDirectory(string souceDirectory)
        {
            if (Directory.Exists(souceDirectory) == false)
            {
                Console.WriteLine(souceDirectory + " does not exist.");
                return false;
            }

            return true;
        }

        private static void ShowHelpMessage()
        {
            Console.WriteLine(@"
====================================
Usage: MSNHistoryCombinerConsole.exe [-s Path(use comma to split)] | [-t TargetDiretoryPath] | [-h |-?]
-s: 
    MSN History Directories to Merge.
-t:
    MSN History Directory to save after merging.
-h:
-?:
    help
====================================
");
        }
    }
}
