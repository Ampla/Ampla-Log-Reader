
using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    /// <summary>
    ///     Extracts the Get Views Parameters from the WcfCall
    /// </summary>
    public class SubmitDataParameter : WcfLocationParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetViewsParameter"/> class.
        /// </summary>
        /// <param name="wcfCall">The WCF call.</param>
        public SubmitDataParameter(WcfCall wcfCall)
        {
            if (wcfCall.Method == "SubmitData")
            {
                WcfResponseMessage responseMessage = new WcfResponseMessage(wcfCall.RequestMessage);
                if (responseMessage.MoveToNode("//*[name()='SubmitDataRequest']"))
                {
                    Operation = "UpdateData";

                    GetCredentials(responseMessage);

                    if (responseMessage.MoveToNode("*[name()='SubmitDataRecords']"))
                    {
                        foreach (XmlNode record in responseMessage.GetXmlNodes("*[name()='Records']"))
                        {
                            List<string> metaData = new List<string>();
                            Location = XmlHelper.GetValue<string>(record, "*[name()='Location']", null);
                            Module = XmlHelper.GetValue<string>(record, "*[name()='Module']", null);

                            string setId = XmlHelper.GetValue<string>(record, "*[name()='MergeCriteria']/*[name()='SetId']", null);
                            if (!string.IsNullOrEmpty(setId))
                            {
                                metaData.Add("SetId={" + setId + "}");
                            }

                            foreach (
                                XmlNode field in
                                    XmlHelper.GetNodes(record, "*[name()='Fields']/*[name()='Field']"))
                            {
                                string name = XmlHelper.GetValue(field, "*[name()='Name']", string.Empty);
                                string value = XmlHelper.GetValue(field, "*[name()='Value']", string.Empty);
                                metaData.Add(name + "={" + value + "}");
                            }
                            MetaData = string.Join(", ", metaData);
                        }
                    }
                }
            }
        }
    }
}