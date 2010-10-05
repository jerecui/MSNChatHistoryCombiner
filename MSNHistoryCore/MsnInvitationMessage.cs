using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnInvitationMessage: MsnMessage
    {
        public MsnInvitationMessage(XmlNode node)
            : base(node)
        {

        }

        public MsnDirection From { get; set; }

        
        public string File { get; set; }


        public string Application { get; set; }
    }
}
