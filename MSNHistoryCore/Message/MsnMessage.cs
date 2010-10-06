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
                this.To = new MsnDirection(this.MessageXmlNode.SelectSingleNode("To"), MsnDirectionType.To);
            }
        }

        public MsnDirection To { get; set; }

        /// <summary>
        /// For some reason, maybe we have duplicated message but combine them together
        /// here for remove them
        /// </summary>
        internal bool IsDuplicated { get; set; }

        public override void AppendOwnSpecial(XmlElement messageElement)
        {
            var xmlDocument = messageElement.OwnerDocument;

            var toElement = xmlDocument.CreateElement(this.To.Direction.ToString());
            this.To.User.ForEach(toUser =>
            {
                var toUserElement = xmlDocument.CreateElement("User");
                toUserElement.SetAttribute("FriendlyName", toUser.FriendlyName);
                toElement.AppendChild(toUserElement);
            });
            messageElement.AppendChild(toElement);
        }
    }
}
