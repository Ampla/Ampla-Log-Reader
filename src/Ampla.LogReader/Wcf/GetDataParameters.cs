
using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class GetDataParameter : IWcfLocationParameter
    {
        public GetDataParameter(WcfCall wcfCall)
        {
            WcfResponseMessage responseMessage = new WcfResponseMessage(wcfCall.RequestMessage);
            if (responseMessage.MoveToNode("//*[name()='GetDataRequest']"))
            {
                Operation = "GetData";

                foreach (XmlNode node in responseMessage.GetXmlNodes("*[name()='Credentials']"))
                {
                    string user = XmlHelper.GetValue(node, "*[name()='Username']", string.Empty);
                    string session = XmlHelper.GetValue(node, "*[name()='Session']", string.Empty);

                    if (!string.IsNullOrEmpty(user))
                    {
                        Credentials = "User: " + user;
                    }
                    else if (!string.IsNullOrEmpty(session))
                    {
                        Credentials = "Session: " + session;
                    }
                    else
                    {
                        Credentials = "(Windows Integrated)";
                    }
                }

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

        public string Credentials { get; private set; }
        public string Operation { get; private set; }
        public string Location { get; private set; }
        public string Module { get; private set; }
        public string MetaData { get; private set; }
    }
}