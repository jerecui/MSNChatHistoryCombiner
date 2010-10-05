using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MsnHistoryCore
{
    public class MsnMessageFactory
    {
        public static readonly string NAMESPACENAME = typeof(MsnMessageFactory).Namespace;

        public static MsnMessageBase Create(XmlNode node)
        {
            if (node == null) throw new ArgumentNullException("The node is invalid");

            var msgClassName = NAMESPACENAME + ".Msn" + node.Name;
            var msgType = Type.GetType(msgClassName);

            var instance = default(MsnMessageBase);
            if (msgType == null)
            {
                InternalLogger.LogInstance.Write(msgClassName + " class does not exist! The file uri is " + node.BaseURI);
            }
            else
            {
                instance = Activator.CreateInstance(msgType, node) as MsnMessageBase;
            }

            return instance;
        }
    }
}
