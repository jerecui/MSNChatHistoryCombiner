using MsnHistoryCore;
using NUnit.Framework;

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

        [Test]
        public void IsMatchedMsnHistoryFile_Valid()
        {
            var file = "scwang30002781717253 - Archive.xml";

            Assert.IsTrue(file.IsMatchedMsnHistoryFile());
        }

        [Test]
        public void IsMatchedMsnHistoryFile_TheFollowingIsInvalid()
        {
            var file = "00000000-0000-0000-0009-b349318be9733605511949.xml";

            Assert.IsFalse(file.IsMatchedMsnHistoryFile());
        }

    }
}
