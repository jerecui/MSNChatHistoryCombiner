using System.Xml;

namespace MsnHistoryCore
{
    public class MsnText
    {
        public MsnText(XmlNode textXmlNode)
        {
            this.TextXmlNode = textXmlNode;

            if (this.TextXmlNode != null)
            {
                if (this.TextXmlNode.Attributes.GetNamedItem("Style") != null)
                    this.Style = this.TextXmlNode.Attributes["Style"].Value;

                this.Value = this.TextXmlNode.InnerText;
            }
        }

        protected XmlNode TextXmlNode { get; set; }

        public string Style { get; set; }

        public string Value { get; set; }
    }
}
