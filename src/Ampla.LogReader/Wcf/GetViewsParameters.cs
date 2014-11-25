
namespace Ampla.LogReader.Wcf
{
    /// <summary>
    ///     Extracts the Get Views Parameters from the WcfCall
    /// </summary>
    public class GetViewsParameter : WcfLocationParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetViewsParameter"/> class.
        /// </summary>
        /// <param name="wcfCall">The WCF call.</param>
        public GetViewsParameter(WcfCall wcfCall)
        {
            if (wcfCall.Method == "GetViews")
            {
                WcfResponseMessage responseMessage = new WcfResponseMessage(wcfCall.RequestMessage);
                if (responseMessage.MoveToNode("//*[name()='GetViewsRequest']"))
                {
                    Operation = "GetViews";

                    GetCredentials(responseMessage);

                    Module = responseMessage.GetXmlValue("*[name()='Module']");
                    Location = responseMessage.GetXmlValue("*[name()='ViewPoint']");
                    MetaData = string.Empty;
                }
            }
        }
    }
}