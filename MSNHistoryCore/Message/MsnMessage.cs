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
