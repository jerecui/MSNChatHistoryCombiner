using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MsnHistoryCore
{
    public class HistoryXmlReader
    {
        public static HistoryXmlReader Instance
        {
            get
            {
                return _instance;
            }
        } private static HistoryXmlReader _instance = new HistoryXmlReader();

        public MsnLog Read(string msnHistoryFile)
        {
            if (string.IsNullOrEmpty(msnHistoryFile)) throw new ArgumentNullException("The file name can not be null");
            if (File.Exists(msnHistoryFile) == false) throw new FileNotFoundException("Can not find file" + msnHistoryFile);

            var xmlDoc = new XmlDocument();
            var xmlReader = new XmlTextReader(msnHistoryFile);
            xmlDoc.Load(xmlReader);

            var log = new MsnLog();

            ReadHead(xmlDoc, log);

            ReadLogBasicProperty(xmlDoc, log);

            var messages = xmlDoc.SelectNodes("Log/Message");
            foreach (XmlNode message in messages)
            {
                log.Messages.Add(new MsnTextMessage(message));
            }

            return log;
        }

        private static void ReadHead(XmlDocument xmlDoc, MsnLog log)
        {
            log.Declaration = xmlDoc.FirstChild as XmlDeclaration;
            log.Xsl = xmlDoc.FirstChild.NextSibling as XmlProcessingInstruction;
        }

        private static void ReadLogBasicProperty(XmlDocument xmlDoc, MsnLog log)
        {
            var logNode = xmlDoc.SelectSingleNode("Log");
            log.FirstSessionID = int.Parse(logNode.Attributes["FirstSessionID"].Value);
            log.LastSessionID = int.Parse(logNode.Attributes["LastSessionID"].Value);
        }

       

    }
}
