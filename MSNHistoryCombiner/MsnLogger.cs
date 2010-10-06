using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsnHistoryCore;
using System.Windows.Forms;

namespace MsnHistoryCombiner
{
    public class MsnLogger : SingletonBase<MsnLogger>, ILogger
    {
        public void Write(string text)
        {
            DisplayLabel.Text = text;
        }

        public ToolStripStatusLabel DisplayLabel { get; set; }

        public bool ShowTime
        {
            get
            {
                return false;
            }
        }
    }
}
