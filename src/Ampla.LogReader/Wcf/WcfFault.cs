using System;
using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfFault
    {
        private static readonly string[] Newline = new string[] {"\r\n"};

        public WcfFault(XmlNode faultMessageNode)
        {
            XmlNode faultNode = XmlHelper.GetNode(faultMessageNode, "*[name()='Fault']");
            if (faultNode != null)
            {
                FaultCode = XmlHelper.GetValue<string>(faultNode, "faultcode", null);
                FaultString = XmlHelper.GetValue<string>(faultNode, "faultstring", null);
                XmlNode exceptionNode = XmlHelper.GetNode(faultNode, "detail/*[name()='ExceptionDetail']");
                Details = exceptionNode != null 
                    ? XmlHelper.GetValue<string>(exceptionNode, "*[name()='StackTrace']", null) 
                    : XmlHelper.GetInnerXml(faultNode, "detail/*");

            }

            FaultString = RemoveWhiteSpace(FaultString);
        }

        private string RemoveWhiteSpace(string value)
        {
            if (value != null && value.Contains(Newline[0]))
            {
                string[] parts = value.Split(Newline, StringSplitOptions.RemoveEmptyEntries);
                List<string> cleaned = new List<string>();
                foreach (string part in parts)
                {
                    string clean = part.Trim();
                    if (!string.IsNullOrEmpty(clean))
                    {
                        cleaned.Add(clean);
                    }
                }
                if (cleaned.Count > 0)
                {
                    return string.Join(" ", cleaned);
                }
            }
            return value;
        }

        public string FaultCode { get; private set; }

        public string FaultString { get; private set; }

        public string Details { get; private set; }

    }
}