using System;
using System.Xml;

namespace MsnHistoryCore
{
    public abstract class MsnMessageBase : IComparable<MsnMessageBase>
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

                var fromNode = this.MessageXmlNode.SelectSingleNode("From");
                if (fromNode != null) this.From = new MsnDirection(fromNode, MsnDirectionType.From);

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

        /// <summary>
        /// For some reason, maybe we have duplicated message but combine them together
        /// here for remove them
        /// </summary>
        internal bool IsDuplicated { get; set; }


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

            var xmlDocument = messageElement.OwnerDocument;

            if (this.From != null)
            {
                var fromElement = xmlDocument.CreateElement(this.From.Direction.ToString());
                var fromUserElement = xmlDocument.CreateElement("User");
                fromUserElement.SetAttribute("FriendlyName", this.From.User[0].FriendlyName);
                messageElement.AppendChild(fromElement);
                fromElement.AppendChild(fromUserElement);
            }

            // their's special
            this.AppendOwnSpecial(messageElement);

            // text
            var textElement = xmlDocument.CreateElement("Text");
            if (string.IsNullOrEmpty(this.Text.Style) == false)
                textElement.SetAttribute("Style", this.Text.Style);
            textElement.InnerText = this.Text.Value;

            messageElement.AppendChild(textElement);

            return messageElement;
        }

        public abstract void AppendOwnSpecial(XmlElement messageElement);

        #endregion

        public int CompareTo(MsnMessageBase other)
        {
            if (other == null) return -1;
            var timeDifference = this.UniversalTime.CompareTo(other.UniversalTime);
            var timeSpanSeconds = GetSpanSecondsFromOther(other);

            if (timeSpanSeconds > 10) return timeDifference;

            // from Diff
            var fromUserDiff = -1;
            if (this.From != null && this.From.User != null && this.From.User.Count > 0
                && other.From != null && other.From.User != null && other.From.User.Count > 0)
            {
                fromUserDiff = string.Compare(this.From.User[0].FriendlyName, other.From.User[0].FriendlyName);
            }

            // Text Diff
            var textDiff = -1;
            if (this.Text != null && other.Text != null)
            {
                textDiff = string.Compare(this.Text.Style, other.Text.Style);
                if (textDiff == 0) textDiff = string.Compare(this.Text.Value, other.Text.Value);
            }

            if (fromUserDiff == 0 && textDiff == 0)
                return 0;

            return timeDifference;
        }

        public double GetSpanSecondsFromOther(MsnMessageBase other)
        {
            if (other == null) return new TimeSpan(this.UniversalTime.Ticks).Seconds;

            var spanSeconds = Math.Abs((this.UniversalTime - other.UniversalTime).TotalSeconds);
            return spanSeconds;
        }
    }
}
