using System;
using System.Xml;

namespace Ampla.LogReader.Xml
{
    public static class NamespaceHelper
    {
        private const string xmlSoap = "http://schemas.xmlsoap.org/soap/envelope/";

        private const string w3Soap = "http://www.w3.org/2003/05/soap-envelope";

        /// <summary>
        /// Determines whether the node is part of the XmlSoap envelope namespace.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <returns></returns>
        public static bool IsXmlSoapNode(XmlNode xmlNode)
        {
            string xmlNamespace = xmlNode.NamespaceURI;
            return StringComparer.InvariantCulture.Compare(xmlSoap, xmlNamespace) == 0;
        }

        /// <summary>
        /// Determines whether the node is part of the W3 Soap envelope namespace.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <returns></returns>
        public static bool IsW3SoapNode(XmlNode xmlNode)
        {
            string xmlNamespace = xmlNode.NamespaceURI;
            return StringComparer.InvariantCulture.Compare(w3Soap, xmlNamespace) == 0;
        }
    }
}