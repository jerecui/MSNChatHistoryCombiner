using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MsnHistoryCore;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class HistoryXmlReaderTests
    {
        public const string MessageOnlyFileName = "MessageOnly.xml";
        public const string InvitationOnlyFileName = "InvitationOnly.xml";

        [Test]
        public void Read_Anything_ReturnCorrectLogObject()
        {
            var log = HistoryXmlReader.Instance.Read(MessageOnlyFileName);

            Assert.AreEqual(1, log.FirstSessionID);
            Assert.AreEqual(2, log.LastSessionID);

        }

        [Test]
        public void Read_MessageOnly_ShouldReturnCorrectMessageItemsCount()
        {
            var log = HistoryXmlReader.Instance.Read(MessageOnlyFileName);

            Assert.AreEqual(4, log.Messages.Count);
        }

        [Test]
        public void Read_MessageOnly_FirstNodeIsCorrect()
        {
            var log = HistoryXmlReader.Instance.Read(MessageOnlyFileName);

            var firstMessage = log.Messages[0] as MsnTextMessage;

            Assert.AreEqual("1/20/2010", firstMessage.Date);
            Assert.AreEqual("10:28:44 AM", firstMessage.Time);
            Assert.AreEqual("2010-01-20T18:28:44.418Z", firstMessage.UniversalTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK"));
            Assert.AreEqual(MsnDirectionType.From, firstMessage.From.Direction);
            Assert.AreEqual(1, firstMessage.From.User.Count);
            Assert.AreEqual("Jerry", firstMessage.From.User[0].FriendlyName);
            Assert.AreEqual(MsnDirectionType.To, firstMessage.To.Direction);
            Assert.AreEqual(2, firstMessage.To.User.Count);
            Assert.AreEqual("Hyperion", firstMessage.To.User[0].FriendlyName);
            Assert.AreEqual("Hargen", firstMessage.To.User[1].FriendlyName);
            Assert.AreEqual("font-family:Segoe UI; color:#000000; ", firstMessage.Text.Style);
            Assert.AreEqual("这两天过得好吗", firstMessage.Text.Value);

        }


    }
}
