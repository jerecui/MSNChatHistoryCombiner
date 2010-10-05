using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using MsnHistoryCore;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class MsnHistoryFileScannerTests
    {
        [Test]
        public void Group_BasedOnName()
        {
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\scwang30002781717253 - Archive.xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"D:\scwang30002781717253 - Archive(1).xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\salley13143760055638.xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"D:\salley13143760055638.xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\cchsky1583658416.xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\cchsky1583658416.xml - Archive(1).xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\cchsky1583658416.xml - Archive(2).xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"D:\scwang30002781717253 - Archive(2).xml"] = false;

            var msnHistoryCombiners = MsnHistoryFileScanner.Instance.Group();

            Assert.AreEqual(3, msnHistoryCombiners.Count);
        }
    }
}
