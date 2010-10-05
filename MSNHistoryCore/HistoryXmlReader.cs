using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MsnHistoryCore
{
    public class HistoryXmlReader:SingletonBase<HistoryXmlReader>
    {
        public HistoryXmlReader()
        {

        }

        public MsnLog Read(string msnHistoryFile)
        {
            if (string.IsNullOrEmpty(msnHistoryFile)) throw new ArgumentNullException("The file name can not be null");
            if (File.Exists(msnHistoryFile) == false) throw new FileNotFoundException("Can not find file" + msnHistoryFile);

            var xmlDoc = new XmlDocument();
            var xmlReader = new XmlTextReader(msnHistoryFile);
            xmlDoc.Load(xmlReader);

            var log = new MsnLog(){XmlFilePath = new FileInfo(msnHistoryFile).FullName};
            ReadHead(xmlDoc, log);

            var rootNode = ReadLogBasicProperty(xmlDoc, log);

            ReadMessages(log, rootNode);

            return log;
        }

        #region Private
        private static void ReadHead(XmlDocument xmlDoc, MsnLog log)
        {
            log.Declaration = xmlDoc.FirstChild as XmlDeclaration;
            log.Xsl = xmlDoc.FirstChild.NextSibling as XmlProcessingInstruction;
        }

        private static XmlNode ReadLogBasicProperty(XmlDocument xmlDoc, MsnLog log)
        {
            var logNode = xmlDoc.SelectSingleNode("Log");
            log.FirstSessionID = int.Parse(logNode.Attributes["FirstSessionID"].Value);
            log.LastSessionID = int.Parse(logNode.Attributes["LastSessionID"].Value);

            return logNode;
        }


        private static void ReadMessages(MsnLog log, XmlNode rootNode)
        {
            var messages = rootNode.SelectNodes("*");
            foreach (XmlNode msgNode in messages)
            {
                log.Messages.Add(MsnMessageFactory.Create(msgNode));
            }
        }
        #endregion
    }
}
