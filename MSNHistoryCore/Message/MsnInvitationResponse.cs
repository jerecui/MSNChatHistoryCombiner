using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public sealed class MsnInvitationResponse : MsnInvitation
    {
        public MsnInvitationResponse(XmlNode node)
            : base(node)
        {

        }
    }
}
