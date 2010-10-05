using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnMessage : MsnMessageBase
    {
        public MsnMessage(XmlNode messageXmlNode)
            : base(messageXmlNode)
        {
            if (this.MessageXmlNode != null)
            {
                this.From = new MsnDirection(this.MessageXmlNode.SelectSingleNode("From"), MsnDirectionType.From);
                this.To = new MsnDirection(this.MessageXmlNode.SelectSingleNode("To"), MsnDirectionType.To);
            }
        }

        public MsnDirection From { get; set; }

        public MsnDirection To { get; set; }
    }
}
