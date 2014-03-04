using System;
using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfFault
    {
        private static readonly string[] Newline = new [] {"\r\n"};

        public WcfFault(XmlNode faultMessageNode)
        {
            XmlNode faultNode = XmlHelper.GetNode(faultMessageNode, "*[name()='Fault']");

            if (faultNode != null)
            {
                if (NamespaceHelper.IsXmlSoapNode(faultNode))
                {
                    FaultCode = XmlHelper.GetValue<string>(faultNode, "faultcode", null);
                    XmlNode stringNode = XmlHelper.GetNode(faultNode, "faultstring");
                    
                    XmlNode detailsNode = XmlHelper.GetNode(faultNode, "detail");

                    FaultString = GetMessage(stringNode, detailsNode);

                    if (detailsNode != null)
                    {
                        XmlNode exceptionNode = XmlHelper.GetNode(detailsNode, "*[name()='ExceptionDetail']");
                        Details = GetDetails(exceptionNode, detailsNode);
                    }
                }

                if (NamespaceHelper.IsW3SoapNode(faultNode))
                {
                    FaultCode = XmlHelper.GetText(faultNode, "*[name() = 'Code']");
                    
                    XmlNode stringNode = XmlHelper.GetNode(faultNode, "*[name()='Reason']");
                    XmlNode detailsNode = XmlHelper.GetNode(faultNode, "*[name()='Detail']");

                    FaultString = GetMessage(stringNode, detailsNode);

                    if (detailsNode != null)
                    {
                        XmlNode exceptionNode = XmlHelper.GetNode(detailsNode, "*[name()='ExceptionDetail']");
                        Details = GetDetails(exceptionNode, detailsNode);
                    }
                }
            }
            
            FaultString = RemoveWhiteSpace(FaultString);
        }

        private string GetMessage(XmlNode stringNode, XmlNode detailNode)
        {
            string message = XmlHelper.GetValue<string>(stringNode, ".", null);
            if (string.IsNullOrEmpty(message))
            {
                message = XmlHelper.GetValue(detailNode, "./descendant::*[name()='Message'][string-length(text()) > 0][1]", string.Empty);
            }
            return message;
        }

        private string GetDetails(XmlNode exceptionNode, XmlNode detailsNode)
        {
            if (exceptionNode != null)
            {
                string type = XmlHelper.GetValue(exceptionNode, "*[name()='Type']", string.Empty);
                string message = XmlHelper.GetValue(exceptionNode, "*[name()='Message']", string.Empty);
                string stackTrace = XmlHelper.GetValue(exceptionNode, "*[name()='StackTrace']", string.Empty);

                string details = string.Format("[{0}] {1}\r\n{2}", type, message, stackTrace);

                XmlNode innerException = XmlHelper.GetNode(exceptionNode, "*[name()='InnerException'][*]");
                if (innerException != null)
                {
                    return details + "\r\n" + GetDetails(innerException, null);
                }
            }
            return detailsNode != null ? XmlHelper.GetInnerXml(detailsNode, ".") : "";
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