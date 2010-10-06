using MsnHistoryCore;
using NUnit.Framework;

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
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\cchsky1583658416 - Archive(1).xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"C:\cchsky1583658416 - Archive(2).xml"] = false;
            MsnHistoryFileScanner.Instance.HistoryRepository[@"D:\scwang30002781717253 - Archive(2).xml"] = false;

            var msnHistoryCombiners = MsnHistoryFileScanner.Instance.Group();

            Assert.AreEqual(3, msnHistoryCombiners.Count);

            var total = 0;
            msnHistoryCombiners.ForEach(item=>total +=item.XmlFilePaths.Count);

            Assert.AreEqual(MsnHistoryFileScanner.Instance.HistoryRepository.Count, total);
        }
    }
}
