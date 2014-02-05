using System;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfCall
    {
        public DateTime CallTime { get; set; }

        public string Url { get; set; }

        public string Action { get; set; }

        public string Method { get; set; }

        public TimeSpan Duration { get; set; }

        public double ResponseMessageLength { get; set; }

        public bool IsFault { get; set; }

        public string FaultMessage { get; set; }

        public string RequestMessage { get; set; }

        public static WcfCall LoadFromXml(XmlNode xmlNode)
        {
            WcfCall call = new WcfCall
                {
                    CallTime = XmlHelper.GetDateTime(xmlNode, "CallTime", DateTime.MinValue),
                    Url = XmlHelper.GetValue(xmlNode, "Url", string.Empty),
                    Action = XmlHelper.GetValue(xmlNode, "Action", string.Empty),
                    Method = XmlHelper.GetValue(xmlNode, "Method", string.Empty),
                    Duration = TimeSpan.FromMilliseconds(XmlHelper.GetValue(xmlNode, "Duration", 0D)),
                    ResponseMessageLength = XmlHelper.GetValue(xmlNode, "ResponseMessageLength", 0D),
                    IsFault = XmlHelper.GetValue(xmlNode, "IsFault", false),
                    FaultMessage = XmlHelper.GetInnerXml(xmlNode, "FaultMessage"),
                    RequestMessage = XmlHelper.GetOuterXml(xmlNode, "RequestMessage"),
                };

            return call;
        }
    }
}