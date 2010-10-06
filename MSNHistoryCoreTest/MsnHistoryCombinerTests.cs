using System.IO;
using MsnHistoryCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
            var combiner = new MsnHistoryCombiner();
            combiner.XmlFilePaths.Add(new FileInfo(TestConsts.FILENAME_COMBINE_LEFT).FullName);
            combiner.XmlFilePaths.Add(new FileInfo(TestConsts.FILENAME_COMBINE_DUPLICATED).FullName);

            combiner.Merge();

            var targetFile = Path.Combine(MsnContext.Instance.TargetDirectoryPath, TestConsts.FILENAME_COMBINE_LEFT.GetHistoryFileUniqueName());
            var mergedLog = HistoryXmlReader.Instance.Read(targetFile);
            Assert.AreEqual(7, mergedLog.Messages.Count);

            File.Delete(targetFile);
        }

        [Test]
        public void RemoveDuplicatedMessage_AllSame_RemoveTheDuplicated()
        {
            var combiner = new MsnHistoryCombiner();
            combiner.MergedMsnLog = new MsnLog();

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null)
            {
                From = new MsnDirection(null, MsnDirectionType.From) { User = new List<MsnUser> { new MsnUser(null) { FriendlyName = "Jerry" } } },
                SessionID = 1,
                UniversalTime = DateTime.Now,
                Text = new MsnText(null) { Style = "bold", Value = "Test the unit" },
            });

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null)
            {
                From = new MsnDirection(null, MsnDirectionType.From) { User = new List<MsnUser> { new MsnUser(null) { FriendlyName = "Jerry" } } },
                SessionID = 1,
                UniversalTime = DateTime.Now.AddMilliseconds(20),
                Text = new MsnText(null) { Style = "bold", Value = "Test the unit" },
            });

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null)
            {
                From = new MsnDirection(null, MsnDirectionType.From) { User = new List<MsnUser> { new MsnUser(null) { FriendlyName = "Jerry" } } },
                SessionID = 1,
                UniversalTime = DateTime.Now.AddMilliseconds(400),
                Text = new MsnText(null) { Style = "bold", Value = "Test the unit" },
            });

            combiner.RemoveDuplicatedMessage();

            Assert.AreEqual(1, combiner.MergedMsnLog.Messages.Count);
        }

        [Test]
        public void RemoveDuplicatedMessage_TextAllSameButTheDiffOfTimeOfFirstAndSecondIsTooBig_JustRemoveTheThird()
        {
            var combiner = new MsnHistoryCombiner();
            combiner.MergedMsnLog = new MsnLog();

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null)
            {
                From = new MsnDirection(null, MsnDirectionType.From) { User = new List<MsnUser> { new MsnUser(null) { FriendlyName = "Jerry" } } },
                SessionID = 1,
                UniversalTime = DateTime.Now,
                Text = new MsnText(null) { Style = "bold", Value = "Test the unit" },
            });

            // This time diff is very big (great than 10 seconds)
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null)
            {
                From = new MsnDirection(null, MsnDirectionType.From) { User = new List<MsnUser> { new MsnUser(null) { FriendlyName = "Jerry" } } },
                SessionID = 1,
                UniversalTime = DateTime.Now.AddSeconds(11),
                Text = new MsnText(null) { Style = "bold", Value = "Test the unit" },
            });

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null)
            {
                From = new MsnDirection(null, MsnDirectionType.From) { User = new List<MsnUser> { new MsnUser(null) { FriendlyName = "Jerry" } } },
                SessionID = 1,
                UniversalTime = DateTime.Now.AddSeconds(12),
                Text = new MsnText(null) { Style = "bold", Value = "Test the unit" },
            });

            combiner.RemoveDuplicatedMessage();

            Assert.AreEqual(2, combiner.MergedMsnLog.Messages.Count);
        }

        [Test]
        public void ResetSessionID_OnlyOneSessionIdExist_NoChange()
        {
            var combiner = new MsnHistoryCombiner();
            combiner.MergedMsnLog = new MsnLog();

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 1, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 1, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 1, UniversalTime = DateTime.Now });

            combiner.ResetSessionID();

            Assert.AreEqual(1, combiner.MergedMsnLog.Messages[combiner.MergedMsnLog.Messages.Count - 1].SessionID);
        }

        [Test]
        public void ResetSessionID_MultipleAndUnOrdered_ReOrder()
        {
            var combiner = new MsnHistoryCombiner();
            combiner.MergedMsnLog = new MsnLog();

            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 1, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 2, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 1, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 2, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 2, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 2, UniversalTime = DateTime.Now });
            combiner.MergedMsnLog.Messages.Add(new MsnMessage(null) { SessionID = 3, UniversalTime = DateTime.Now });

            combiner.ResetSessionID();
            Assert.AreEqual(1, combiner.MergedMsnLog.Messages[0].SessionID);
            Assert.AreEqual(4, combiner.MergedMsnLog.Messages[combiner.MergedMsnLog.Messages.Count - 2].SessionID);
            Assert.AreEqual(5, combiner.MergedMsnLog.Messages[combiner.MergedMsnLog.Messages.Count - 1].SessionID);
        }
    }
}
