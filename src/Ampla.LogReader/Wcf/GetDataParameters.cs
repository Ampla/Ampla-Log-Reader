
using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    /// <summary>
    ///     Extracts the Get Data Parameters from the WcfCall
    /// </summary>
    public class GetDataParameter : WcfLocationParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDataParameter"/> class.
        /// </summary>
        /// <param name="wcfCall">The WCF call.</param>
        public GetDataParameter(WcfCall wcfCall)
        {
            if (wcfCall.Method == "GetData")
            {
                WcfResponseMessage responseMessage = new WcfResponseMessage(wcfCall.RequestMessage);
                if (responseMessage.MoveToNode("//*[name()='GetDataRequest']"))
                {
                    Operation = "GetData";

                    GetCredentials(responseMessage);

                    Module = responseMessage.GetXmlValue("*[name()='View']/*[name()='Module']");

                    if (responseMessage.MoveToNode("*[name()='Filter']"))
                    {
                        List<string> metaData = new List<string>();
                        Location = responseMessage.GetXmlValue("*[name()='Location']");
                        string deleted = responseMessage.GetXmlValue("*[name()='Deleted']");
                        if (!string.IsNullOrEmpty(deleted))
                        {
                            metaData.Add("Deleted={" + deleted + "}");
                        }

                        foreach (XmlNode node in responseMessage.GetXmlNodes("*[name()='Criteria']/*[name()='FilterEntries']"))
                        {
                            string name = XmlHelper.GetValue(node, "*[name()='Name']", string.Empty);
                            string value = XmlHelper.GetValue(node, "*[name()='Value']", string.Empty);
                            metaData.Add(name + "={" + value + "}");
                        }
                        MetaData = string.Join(", ", metaData);
                    }
                }
            }
        }
    }
}