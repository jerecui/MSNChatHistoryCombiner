using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public abstract class MsnMessage
    {
        protected XmlNode MessageXmlNode { get; set; }

        public MsnMessage(XmlNode messageXmlNode)
        {
            this.MessageXmlNode = messageXmlNode;

            if (this.MessageXmlNode != null)
            {
                this.Date = this.MessageXmlNode.Attributes["Date"].Value;
                this.Time = this.MessageXmlNode.Attributes["Time"].Value;
                this.SessionID = int.Parse(this.MessageXmlNode.Attributes["SessionID"].Value);

                this.UniversalTime = DateTime.Parse(this.MessageXmlNode.Attributes["DateTime"].Value).ToUniversalTime();

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
        public MsnText Text { get; set; }
        #endregion

        #endregion
    }
}
