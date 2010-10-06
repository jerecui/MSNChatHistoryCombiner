using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnInvitation : MsnMessageBase
    {
        public MsnInvitation(XmlNode node)
            : base(node)
        {
            if (this.MessageXmlNode != null)
            {

                this.File = this.GetSingleChildInnerText("File");
                this.Application = this.GetSingleChildInnerText("Application");
            }
        }

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

        public override void AppendOwnSpecial(XmlElement messageElement)
        {
            if (string.IsNullOrEmpty(this.File) == false)
            {
                var fileElement = messageElement.OwnerDocument.CreateElement("File");
                fileElement.InnerText = this.File;

                messageElement.AppendChild(fileElement);
            }

            if (string.IsNullOrEmpty(this.Application) == false)
            {
                var applicationElement = messageElement.OwnerDocument.CreateElement("Application");
                applicationElement.InnerText = this.Application;

                messageElement.AppendChild(applicationElement);
            }
        }
    }
}
