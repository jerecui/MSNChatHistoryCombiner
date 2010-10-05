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
            xmlDoc.LoadXml(msnHistoryFile);

            var log = new MsnLog();
            var logNode = xmlDoc.SelectSingleNode("Log");
            //log = logNode.Attributes[log.FirstSessionID]

            return null;
        }

    }
}
