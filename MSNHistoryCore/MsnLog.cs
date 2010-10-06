using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Xsl;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnLog
    {
        public XmlDeclaration Declaration { get; set; }
        public XmlProcessingInstruction Xsl { get; set; }
        public string XmlFilePath { get; set; }
        //internal XmlDocument RawXml { get; set; }

        public int FirstSessionID { get; set; }

        public int LastSessionID { get; set; }

        public List<MsnMessageBase> Messages
        {
            get
            {
                if (_messages == null) _messages = new List<MsnMessageBase>();
                return _messages;
            }
        } private List<MsnMessageBase> _messages;

        public void Save(string fileName)
        {
            if (this.Messages.Count == 0 || string.IsNullOrEmpty(fileName)) return;

            var xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration(this.Declaration.Version, this.Declaration.Encoding, this.Declaration.Standalone));
            xml.AppendChild(xml.CreateProcessingInstruction(this.Xsl.Target, this.Xsl.Data));

            var root = xml.CreateElement("Log");
            root.SetAttribute("FirstSessionID", "1");
            root.SetAttribute("LastSessionID", "1");

            xml.AppendChild(root);
            this.Messages.ForEach(messageNode =>
                {
                    var nodeName = messageNode.GetType().Name.Replace("Msn", string.Empty);
                    var newNode = xml.CreateElement(nodeName);
                    root.AppendChild(messageNode.GenerateXmlNode(newNode));
                });

            var writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.WriteRaw(xml.OuterXml);
            writer.Flush();
            writer.Close();
        }

    }
}
