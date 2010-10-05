using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnUser
    {
        public MsnUser(XmlNode userNode)
        {
            this.UserXmlNode = userNode;

            if (this.UserXmlNode != null)
            {
                this.FriendlyName = this.UserXmlNode.Attributes["FriendlyName"].Value;
            }
        }
        protected XmlNode UserXmlNode { get; set; }


        public string FriendlyName { get; set; }
    }
}
