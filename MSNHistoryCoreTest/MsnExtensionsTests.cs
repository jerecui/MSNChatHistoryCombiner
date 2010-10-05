using NUnit.Framework;
using MsnHistoryCore;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class MsnExtensionsTests
    {
        [Test]
        public void IsSameFriendHistory_Test()
        {
            var fileName = "scwang30002781717253.xml";
            Assert.IsTrue(fileName.IsSameFriendHistory("scwang30002781717253 - Archive.xml"));
            Assert.IsTrue(fileName.IsSameFriendHistory("scwang30002781717253 - Archive (5).xml"));
            Assert.IsTrue(fileName.IsSameFriendHistory("scwang30002781717253 - Archive (2).xml"));
            Assert.IsTrue(fileName.IsSameFriendHistory("scwang30002781717253 -    Archive (2).xml"));
            Assert.IsTrue(fileName.IsSameFriendHistory("scwang30002781717253 - Archive    (2).xml"));
            Assert.IsFalse(fileName.IsSameFriendHistory("scwang50002781717253 - Archive (2).xml"));
        }

    }
}
