using System;
using System.Xml;
using MsnHistoryCore;
using NUnit.Framework;

namespace MsnHistoryCoreTest
{
    [TestFixture]
    public class MsnMessageFactoryTests
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(ArgumentNullException))]
        public void Create_ThrowException_WhenArugemntWrong()
        {
            var node = default(XmlNode);

            var instance = MsnMessageFactory.Create(node);
        }
    }
}
