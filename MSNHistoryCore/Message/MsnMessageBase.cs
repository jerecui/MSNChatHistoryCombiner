using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public abstract class MsnMessageBase
    {
        protected XmlNode MessageXmlNode { get; set; }

        public MsnMessageBase(XmlNode messageXmlNode)
        {
            this.MessageXmlNode = messageXmlNode;

            if (this.MessageXmlNode != null)
            {
                this.Date = this.MessageXmlNode.Attributes["Date"].Value;
                this.Time = this.MessageXmlNode.Attributes["Time"].Value;
                this.SessionID = int.Parse(this.MessageXmlNode.Attributes["SessionID"].Value);

                this.UniversalTime = DateTime.Parse(this.MessageXmlNode.Attributes["DateTime"].Value).ToUniversalTime();

                this.From = new MsnDirection(this.MessageXmlNode.SelectSingleNode("From"), MsnDirectionType.From);

                this.Text = new MsnText(this.MessageXmlNode.SelectSingleNode("Text"));
            }
        }

        #region Properties

        #region Attributes
        public string Date { get; set; }

        public string Time { get; set; }

        public DateTime UniversalTime { get; set; }

        public int SessionID { get; set; }
        #endregion

        #region Child Nodes
        public MsnDirection From { get; set; }

        public MsnText Text { get; set; }
        #endregion

        #endregion

        #region Methods
        public virtual XmlNode GenerateXmlNode(XmlElement messageElement)
        {
            messageElement.SetAttribute("Date", this.Date);
            messageElement.SetAttribute("Time", this.Time);
            messageElement.SetAttribute("DateTime", this.UniversalTime.ToMsnUniversalString());
            messageElement.SetAttribute("SessionID", this.SessionID.ToString());


            //var fromElement = xmlDocument.CreateElement(this.From.Direction.ToString());
            //var fromUserElement = xmlDocument.CreateElement("User");
            //fromUserElement.SetAttribute("FriendlyName", this.From.User[0].FriendlyName);

            //messageElement.AppendChild(fromElement);
            //fromElement.AppendChild(fromUserElement);


            //var toElement = xmlDocument.CreateElement(this.To.Direction.ToString());
            //this.To.User.ForEach(toUser =>
            //{
            //    var toUserElement = xmlDocument.CreateElement("User");
            //    toUserElement.SetAttribute("FriendlyName", toUser.FriendlyName);
            //    toElement.AppendChild(toUserElement);
            //});
            //messageElement.AppendChild(toElement);

            var xmlDocument = messageElement.OwnerDocument;

            var fromElement = xmlDocument.CreateElement(this.From.Direction.ToString());
            var fromUserElement = xmlDocument.CreateElement("User");
            fromUserElement.SetAttribute("FriendlyName", this.From.User[0].FriendlyName);
            messageElement.AppendChild(fromElement);
            fromElement.AppendChild(fromUserElement);

            // their's special
            this.AppendOwnSpecial(messageElement);

            // text
            var textElement = xmlDocument.CreateElement("Text");
            textElement.SetAttribute("Style", this.Text.Style);
            textElement.InnerText = this.Text.Value;

            messageElement.AppendChild(textElement);

            return messageElement;
        }

        public abstract void AppendOwnSpecial(XmlElement messageElement);

        #endregion
    }
}
