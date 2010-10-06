using System.Collections.Generic;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnDirection
    {
        public MsnDirection(XmlNode directionNode, MsnDirectionType direction)
        {
            this.Direction = direction;
            this.DirectionNode = directionNode;

            if (this.DirectionNode != null)
            {
                var users = this.DirectionNode.SelectNodes("User");

                foreach (XmlNode user in users)
                {
                    this.User.Add(new MsnUser(user));
                }
            }
        }

        protected XmlNode DirectionNode { get; set; }

        public MsnDirectionType Direction { get; set; }

        public List<MsnUser> User
        {
            get
            {
                if (_user == null) _user = new List<MsnUser>();
                return _user;
            }
            internal set
            {
                _user = value;
            }
        } private List<MsnUser> _user;

    }
}
