using System;
using System.Xml;

namespace Ampla.LogReader.Xml
{
    public static class XmlHelper
    {
        public static T GetValue<T>(XmlNode xmlNode, string xPath, T defaultValue)
        {
            if (typeof (T) == typeof (DateTime))
            {
                throw new InvalidOperationException("Use GetDateTime() to get DateTime values");
            }

            XmlNode valueNode = xmlNode.SelectSingleNode(xPath);

            if (valueNode != null)
            {
                string value = valueNode.InnerText;
                //if (!string.IsNullOrEmpty(value))
                {
                    return (T) Convert.ChangeType(value, typeof (T));
                }
            }
            return defaultValue;
        }

        public static DateTime GetDateTime(XmlNode xmlNode, string xPath, DateTime defaultValue)
        {
            XmlNode valueNode = xmlNode.SelectSingleNode(xPath);
            if (valueNode != null)
            {
                string value = valueNode.InnerText;
                if (!string.IsNullOrEmpty(value))
                {
                    return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc);
                }
            }
            return defaultValue;
        }

        public static string GetOuterXml(XmlNode xmlNode, string xPath)
        {
            XmlNode node = xmlNode.SelectSingleNode(xPath);
            return node != null ? node.OuterXml : null;
        }
    }
}