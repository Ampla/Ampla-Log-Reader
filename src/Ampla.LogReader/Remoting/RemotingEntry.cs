using System;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingEntry
    {
        public DateTime CallTime { get; set; }

        public string Identity { get; set; }
            
        public string Method { get; set; }

        public string TypeName { get; set; }

        public TimeSpan Duration { get; set; }

        public string Arguments { get; set; }

        public static RemotingEntry LoadFromXml(XmlNode xmlNode)
        {
            RemotingEntry entry = new RemotingEntry
            {
                CallTime = XmlHelper.GetDateTime(xmlNode, "UTCDateTime", DateTime.MinValue),
                Identity = XmlHelper.GetValue(xmlNode, "Identity", string.Empty),
                Method = XmlHelper.GetValue(xmlNode, "__MethodName", string.Empty),
                TypeName = XmlHelper.GetValue(xmlNode, "__TypeName", string.Empty),
                Duration = TimeSpan.FromMilliseconds(XmlHelper.GetValue(xmlNode, "__MessageResponseTime", 0D)),
                Arguments = XmlHelper.GetInnerXml(xmlNode, "__Args"),
            };

            return entry;
        }
 
    }
}