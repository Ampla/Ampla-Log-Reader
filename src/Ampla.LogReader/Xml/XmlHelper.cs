using System;
using System.Xml;

namespace Ampla.LogReader.Xml
{
    public static class XmlHelper
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Use GetDateTime() to get DateTime values</exception>
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

        /// <summary>
        /// Gets the date time as UTC Value
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeUtc(XmlNode xmlNode, string xPath, DateTime defaultValue)
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

        /// <summary>
        /// Gets the date time as Local Value
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime GetDateTimeLocal(XmlNode xmlNode, string xPath, DateTime defaultValue)
        {
            XmlNode valueNode = xmlNode.SelectSingleNode(xPath);
            if (valueNode != null)
            {
                string value = valueNode.InnerText;
                if (!string.IsNullOrEmpty(value))
                {
                    return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Local);
                }
            }
            return defaultValue;
        }


        /// <summary> 
        /// Gets the outer XML from the xPath specified of xmlNode
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <returns></returns>
        public static string GetOuterXml(XmlNode xmlNode, string xPath)
        {
            XmlNode node = xmlNode.SelectSingleNode(xPath);
            return node != null ? node.OuterXml : null;
        }

        /// <summary> 
        /// Gets the inner XML from the xPath specified of xmlNode
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <returns></returns>
        public static string GetInnerXml(XmlNode xmlNode, string xPath)
        {
            XmlNode node = xmlNode.SelectSingleNode(xPath);
            return node != null ? node.InnerXml : null;
        }

        /// <summary>
        /// Gets the xmlNode at the specified xPath.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <returns></returns>
        public static XmlNode GetNode(XmlNode xmlNode, string xPath)
        {
            XmlNode node = xmlNode.SelectSingleNode(xPath);
            return node;
        }

        /// <summary>
        /// Gets the xmlNode at the specified xPath.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <returns></returns>
        public static XmlNodeList GetNodes(XmlNode xmlNode, string xPath)
        {
            XmlNodeList nodeList = xmlNode.SelectNodes(xPath);
            return nodeList;
        }

        /// <summary>
        /// Gets the text nodes from the specified xPath node
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="xPath">The executable path.</param>
        /// <returns></returns>
        public static string GetText(XmlNode xmlNode, string xPath)
        {
            XmlNode node = xmlNode.SelectSingleNode(xPath);
            return node != null ? node.InnerText : null;
        }

        /// <summary>
        /// Gets the namespace for the node
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <returns></returns>
        public static string GetNamespace(XmlNode xmlNode)
        {
            return xmlNode.NamespaceURI;
        }
    }
}