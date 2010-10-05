using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnInvitation: MsnMessageBase
    {
        public MsnInvitation(XmlNode node)
            : base(node)
        {
            if (this.MessageXmlNode != null)
            {
                this.From = new MsnDirection(this.MessageXmlNode.SelectSingleNode("From"), MsnDirectionType.From);

                this.File = this.GetSingleChildInnerText("File");
                this.Application = this.GetSingleChildInnerText("Application");
            }
        }

        public MsnDirection From { get; set; }
        
        public string File { get; set; }

        public string Application { get; set; }

        private string GetSingleChildInnerText(string childNodeName)
        {
            var childNode = this.MessageXmlNode.SelectSingleNode(childNodeName);
            var childNodeInnerTextValue = default(string);
            if (childNode != null)
            {
                childNodeInnerTextValue = childNode.InnerText;
            }

            return childNodeInnerTextValue;
        }
    }
}
