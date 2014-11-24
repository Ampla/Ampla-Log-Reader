using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfResponseMessage
    {
        private readonly XmlDocument xmlDoc;
        private XmlNode xmlNode;

        public WcfResponseMessage(string responseMessage)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseMessage);
            xmlNode = xmlDoc;
        }

        public string[] GetNames()
        {
            List<string> names = new List<string>();
            foreach (XmlNode xmlChild in XmlHelper.GetNodes(xmlNode, "*"))
            {
                names.Add(xmlChild.Name);
            }
            return names.ToArray();
        }

        public bool MoveToNode(string xPath)
        {
            XmlNode newNode = XmlHelper.GetNode(xmlNode, xPath);
            if (newNode != null)
            {
                xmlNode = newNode;
                return true;
            }
            return false;
        }

        public XmlNodeList GetXmlNodes(string xPathNodes)
        {
            return XmlHelper.GetNodes(xmlNode, xPathNodes);
        }

        public string GetXmlValue(string xPath)
        {
            return XmlHelper.GetValue<string>(xmlNode, xPath, null);
        }
    }
}