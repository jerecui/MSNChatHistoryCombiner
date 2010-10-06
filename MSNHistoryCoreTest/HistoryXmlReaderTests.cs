using MsnHistoryCore;
using NUnit.Framework;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class HistoryXmlReaderTests
    {
      

        [Test]
        public void Read_Anything_ReturnCorrectLogObject()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_MESSAGEONLY);

            Assert.AreEqual(1, log.FirstSessionID);
            Assert.AreEqual(2, log.LastSessionID);

        }

        [Test]
        public void Read_MessageOnly_ShouldReturnCorrectMessageItemsCount()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_MESSAGEONLY);

            Assert.AreEqual(4, log.Messages.Count);
        }

        [Test]
        public void Read_MessageOnly_FirstNodeIsCorrect()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_MESSAGEONLY);

            var firstMessage = log.Messages[0] as MsnMessage;

            Assert.AreEqual("1/20/2010", firstMessage.Date);
            Assert.AreEqual("10:28:44 AM", firstMessage.Time);
            Assert.AreEqual("2010-01-20T18:28:44.418Z", firstMessage.UniversalTime.ToMsnUniversalString());
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

        [Test]
        public void Read_InvitatoinOnly_ShouldReturnCorrectItemCount()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_INVITATIONONLY);

            var itemCount = log.Messages.Count;

            Assert.AreEqual(5, itemCount);
        }

        [Test]
        public void Read_InvitatoinOlny_InvitationItemDetailInformationIsCorrect()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_INVITATIONONLY);

            var invitaion = log.Messages[0] as MsnInvitation;

            Assert.IsNotNull(invitaion);
            Assert.AreEqual("2/4/2010", invitaion.Date);
            Assert.AreEqual("2:37:42 PM", invitaion.Time);
            Assert.AreEqual("2010-02-04T06:37:42.403Z", invitaion.UniversalTime.ToMsnUniversalString());
            Assert.AreEqual(MsnDirectionType.From, invitaion.From.Direction);
            Assert.AreEqual(1, invitaion.From.User.Count);
            Assert.AreEqual("Hargen", invitaion.From.User[0].FriendlyName);
            Assert.AreEqual("3ut18.tmp.jpg", invitaion.File);
            Assert.AreEqual("Hargen sends 3ut18.tmp.jpg", invitaion.Text.Value);
        }

        [Test]
        public void Read_InvitatoinOlny_InvitationResponseItemDetailInformationIsCorrect()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_INVITATIONONLY);

            var invitaion = log.Messages[3] as MsnInvitation;

            Assert.IsNotNull(invitaion);
            Assert.AreEqual("2/2/2010", invitaion.Date);
            Assert.AreEqual("11:36:42 AM", invitaion.Time);
            Assert.AreEqual("2010-02-02T03:36:42.057Z", invitaion.UniversalTime.ToMsnUniversalString());
            Assert.AreEqual(MsnDirectionType.From, invitaion.From.Direction);
            Assert.AreEqual(1, invitaion.From.User.Count);
            Assert.AreEqual("菲児", invitaion.From.User[0].FriendlyName);
            Assert.AreEqual("a Computer Call", invitaion.Application);
            Assert.AreEqual("You missed a call from 菲児.", invitaion.Text.Value);
        }


        //////////////////////////////
        [Test]
        public void Read_LeaveOnly_ShouldReturnCorrectItemCount()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_LEAVEONLY);

            var itemCount = log.Messages.Count;

            Assert.AreEqual(1, itemCount);
        }

        [Test]
        public void Read_LeaveOnly_DetailInformationIsCorrect()
        {
            var log = HistoryXmlReader.Instance.Read(TestConsts.FILENAME_LEAVEONLY);

            var leaveMessage = log.Messages[0] as MsnLeave;

            Assert.AreEqual("9/12/2006", leaveMessage.Date);
            Assert.AreEqual("1:34:22 PM", leaveMessage.Time);
            Assert.AreEqual("2006-09-12T05:34:22.671Z", leaveMessage.UniversalTime.ToMsnUniversalString());
            Assert.AreEqual("柴松洁", leaveMessage.User.FriendlyName);
            Assert.AreEqual("柴松洁 has left the conversation.", leaveMessage.Text.Value);
        }
    }
}
