using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml;
using MsnHistoryCore;

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
