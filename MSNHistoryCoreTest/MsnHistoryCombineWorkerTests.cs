using MsnHistoryCore;
using NUnit.Framework;
using System.IO;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class MsnHistoryCombineWorkerTests
    {
        [Test]
        public void Group_BasedOnName()
        {
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"C:\scwang30002781717253 - Archive.xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"D:\scwang30002781717253 - Archive(1).xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"C:\salley13143760055638.xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"D:\salley13143760055638.xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"C:\cchsky1583658416.xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"C:\cchsky1583658416 - Archive(1).xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"C:\cchsky1583658416 - Archive(2).xml"] = false;
            MsnHistoryCombineWorker.Instance.HistoryRepository[@"D:\scwang30002781717253 - Archive(2).xml"] = false;

            var msnHistoryCombiners = MsnHistoryCombineWorker.Instance.Group();

            Assert.AreEqual(3, msnHistoryCombiners.Count);

            var total = 0;
            msnHistoryCombiners.ForEach(item => total += item.XmlFilePaths.Count);

            Assert.AreEqual(MsnHistoryCombineWorker.Instance.HistoryRepository.Count, total);
        }

        [Test]
        public void CopyXslFile_Test()
        {
            MsnContext.Instance.TargetDirectoryPath = @"D:\temp";

            var targetDirInfo = new DirectoryInfo(MsnContext.Instance.TargetDirectoryPath);
            var targetFile = Path.Combine(targetDirInfo.FullName, "MessageLog.xsl");
            if (File.Exists(targetFile)) File.Delete(targetFile);

            MsnHistoryCombineWorker.Instance.CopyXslFile();
            Assert.IsTrue(File.Exists(targetFile));

            File.Delete(targetFile);
        }
    }
}
