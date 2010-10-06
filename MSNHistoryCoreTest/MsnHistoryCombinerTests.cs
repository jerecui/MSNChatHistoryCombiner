using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MsnHistoryCore;
using System.IO;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class MsnHistoryCombinerTests
    {
        [SetUp]
        public void Setup()
        {
            MsnContext.Instance.TargetDirectoryPath = @"D:\temp\msn\merged";
        }

        [Test]
        public void SouceLogs_ShouldRemoveNoExistedFile()
        {
            var existedFile = new FileInfo(TestConsts.FILENAME_MESSAGEONLY).FullName;
            var noExistedFile = @"C:\scwang30002781717253 - Archive(100).xml";

            var combiner = new MsnHistoryCombiner();
            combiner.XmlFilePaths.Add(existedFile);
            combiner.XmlFilePaths.Add(noExistedFile);

            Assert.AreEqual(1, combiner.SourceLogs.Count);
        }

        [Test]
        public void Merge_OnlyOneSource_CopiedToMergedDirectoryDirectly()
        {
            var combiner = new MsnHistoryCombiner();
            combiner.XmlFilePaths.Add((new FileInfo(TestConsts.FILENAME_COMBINE_LEFT)).FullName);

            combiner.Merge();

            // find the result
            var targetFile = Path.Combine(MsnContext.Instance.TargetDirectoryPath, TestConsts.FILENAME_COMBINE_LEFT.GetHistoryFileUniqueName());
            Assert.IsTrue(File.Exists(targetFile));

            var mergedLog = HistoryXmlReader.Instance.Read(targetFile);
            Assert.AreEqual(7, mergedLog.Messages.Count);

            // File delete
            File.Delete(targetFile);
        }

        [Test]
        public void Merge_TwoSouces_MergeTheMessageTogether()
        {
            var combiner = new MsnHistoryCombiner();
            combiner.XmlFilePaths.Add((new FileInfo(TestConsts.FILENAME_COMBINE_LEFT)).FullName);
            combiner.XmlFilePaths.Add((new FileInfo(TestConsts.FILENAME_COMBINE_RIGHT)).FullName);

            combiner.Merge();

            // find the result
            var targetFile = Path.Combine(MsnContext.Instance.TargetDirectoryPath, TestConsts.FILENAME_COMBINE_LEFT.GetHistoryFileUniqueName());
            Assert.IsTrue(File.Exists(targetFile));

            var mergedLog = HistoryXmlReader.Instance.Read(targetFile);
            Assert.AreEqual(17, mergedLog.Messages.Count);

            File.Delete(targetFile);
        }

        [Test]
        public void Merge_TwoSameSources_RemoveDuplicatedMessages()
        {

        }
    }
}
