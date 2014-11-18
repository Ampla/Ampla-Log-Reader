using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfCall
    {
        public DateTime CallTime { get; private set; }

        public string Url { get; private set; }

        public string Action { get; private set; }

        public string Method { get; private set; }

        public TimeSpan Duration { get; private set; }

        public double ResponseMessageLength { get; private set; }

        public bool IsFault { get; private set; }

        public WcfFault Fault { get; private set; }

        public string RequestMessage { get; private set; }

        public string Source { get; set; }

        public static WcfCall LoadFromXml(XmlNode xmlNode)
        {
            WcfCall call = new WcfCall
                {
                    CallTime = XmlHelper.GetDateTimeUtc(xmlNode, "CallTime", DateTime.MinValue),
                    Url = XmlHelper.GetValue(xmlNode, "Url", string.Empty),
                    Action = XmlHelper.GetValue(xmlNode, "Action", string.Empty),
                    Method = XmlHelper.GetValue(xmlNode, "Method", string.Empty),
                    Duration = TimeSpan.FromMilliseconds(XmlHelper.GetValue(xmlNode, "Duration", 0D)),
                    ResponseMessageLength = XmlHelper.GetValue(xmlNode, "ResponseMessageLength", 0D),
                    IsFault = XmlHelper.GetValue(xmlNode, "IsFault", false),
                    Fault = null,
                    RequestMessage = XmlHelper.GetOuterXml(xmlNode, "RequestMessage"),
                };

            if (call.IsFault)
            {
                XmlNode faultNode = XmlHelper.GetNode(xmlNode, "FaultMessage");
                if (faultNode != null)
                {
                    call.Fault = new WcfFault(faultNode);
                }
            }

            if (string.IsNullOrEmpty(call.Method))
            {
                call.Method = call.Action != null && call.Action.Contains("/")
                                  ? call.Action.Substring(call.Action.LastIndexOf('/') + 1)
                                  : call.Action;
            }

            return call;
        }

    }
}