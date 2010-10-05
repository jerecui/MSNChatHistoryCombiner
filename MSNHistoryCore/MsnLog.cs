using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Xsl;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnLog
    {
        public XmlDeclaration Declaration { get; set; }
        public XmlProcessingInstruction Xsl { get; set; }

        public int FirstSessionID { get; set; }

        public int LastSessionID { get; set; }

        public List<MsnMessage> Messages
        {
            get
            {
                if (_messages == null) _messages = new List<MsnMessage>();
                return _messages;
            }
        } private List<MsnMessage> _messages;
    }
}
