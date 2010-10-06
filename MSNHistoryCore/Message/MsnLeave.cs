using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnLeave : MsnMessageBase
    {
        public MsnLeave(XmlNode messageXmlNode)
            : base(messageXmlNode)
        {
            if (this.MessageXmlNode != null)
            {
                this.User = new MsnUser(this.MessageXmlNode.SelectSingleNode("User"));
            }
        }

        public MsnUser User { get; set; }

        public override void AppendOwnSpecial(System.Xml.XmlElement messageElement)
        {
            var xmlDocument = messageElement.OwnerDocument;

            var userElement = xmlDocument.CreateElement("User");
            userElement.SetAttribute("FriendlyName", this.User.FriendlyName);

            messageElement.AppendChild(userElement);
        }
    }
}
