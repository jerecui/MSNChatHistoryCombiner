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
        public const string MessageOnlyFileName = "MessageOnly";
        public const string InvitationOnlyFileName = "InvitationOnly";

        [Test]
        public void Read_Anything_ReturnCorrectLogObject()
        {
            var log = HistoryXmlReader.Instance.Read(MessageOnlyFileName);

            Assert.AreEqual(1, log.FirstSessionID);
            Assert.AreEqual(2, log.LastSessionID);

        }

        public void Read_MessageOnly_ShouldReturnCorrectMessageItems()
        {

        }



    }
}
