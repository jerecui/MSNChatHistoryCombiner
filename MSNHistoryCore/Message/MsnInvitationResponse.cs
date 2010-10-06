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
