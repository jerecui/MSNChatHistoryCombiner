using System.Windows.Forms;
using MsnHistoryCore;

namespace MsnHistoryCombiner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnMerge_Click(object sender, System.EventArgs e)
        {
            var path1 = @"D:\Documents\MSNHistory";
            var path2 = @"E:\Document\FL-LT\MSNHistory";

            MsnContext.Instance.SourceDirectories.Add(path1);
            MsnContext.Instance.SourceDirectories.Add(path2);
            MsnContext.Instance.TargetDirectoryPath = @"D:\temp\msn\merged";

            MsnLogger.Instance.DisplayLabel = this.statusLable;
            InternalLogger.InitializeLogger(MsnLogger.Instance);

            MsnHistoryFileScanner.Instance.Scan();
            MsnHistoryFileScanner.Instance.Merge();
        }
    }
}
