using System;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingEntry
    {
        private RemotingEntry()
        {
        }

        private static readonly char[] Comma = new[] { ',' };
        
        public DateTime CallTimeUtc { get; private set; }

        public DateTime CallTimeLocal { get; private set; }

        public string Identity { get; private set; }
            
        public string Method { get; private set; }

        public string TypeName { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string ArgumentXml { get; private set; }

        public RemotingArgument[] Arguments { get; private set; } 

        public static RemotingEntry LoadFromXml(XmlNode xmlNode)
        {
            RemotingEntry entry = new RemotingEntry
            {
                CallTimeUtc = XmlHelper.GetDateTimeUtc(xmlNode, "UTCDateTime", DateTime.MinValue),
                CallTimeLocal = XmlHelper.GetDateTimeLocal(xmlNode, "LocalDateTime", DateTime.MinValue),
                Identity = XmlHelper.GetValue(xmlNode, "Identity", string.Empty),
                Method = XmlHelper.GetValue(xmlNode, "__MethodName", string.Empty),
                TypeName = XmlHelper.GetValue(xmlNode, "__TypeName", string.Empty),
                Duration = TimeSpan.FromMilliseconds(XmlHelper.GetValue(xmlNode, "__MessageResponseTime", 0D)),
                ArgumentXml = XmlHelper.GetInnerXml(xmlNode, "__Args"),
                Arguments = RemotingArgument.Parse(XmlHelper.GetNodes(xmlNode, "__Args/*")),
            };

            if (!string.IsNullOrEmpty(entry.TypeName))
            {
                entry.TypeName = entry.TypeName.Split(Comma, StringSplitOptions.None)[0];
            }
            
            return entry;
        }

    }
}