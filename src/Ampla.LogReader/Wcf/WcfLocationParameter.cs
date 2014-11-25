using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    /// <summary>
    ///     Base class for WcfCall Location Parameters
    /// </summary>
    public abstract class WcfLocationParameter : IWcfLocationParameter
    {
        public string Credentials { get; private set; }
        public string Operation { get; protected set; }
        public string Location { get; protected set; }
        public string Module { get; protected set; }
        public string MetaData { get; protected set; }
        
        /// <summary>
        /// Gets the credentials from the WcfResponseMessage
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        protected void GetCredentials(WcfResponseMessage responseMessage)
        {
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
        }
    }
}