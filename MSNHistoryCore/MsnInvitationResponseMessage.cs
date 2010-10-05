using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public sealed class MsnInvitationResponseMessage : MsnInvitationMessage
    {
        public MsnInvitationResponseMessage(XmlNode node)
            : base(node)
        {

        }
    }
}
