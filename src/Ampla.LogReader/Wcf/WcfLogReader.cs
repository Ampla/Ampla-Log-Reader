using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfLogReader : LogReader<WcfCall>
    {
        private readonly string fileName;

        public WcfLogReader(string fileName)
        {
            this.fileName = fileName;
        }

        protected override List<WcfCall> ReadEntries()
        {
            List<WcfCall> wcfCalls = new List<WcfCall>();
            XmlContentTextReader contentReader = new XmlContentTextReader("<WCFCall>", File.OpenRead(fileName));
            XmlFragmentTextReader reader = new XmlFragmentTextReader("Xml", contentReader);
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(reader);
            }
            catch (Exception ex)
            {
                throw new XmlException("Error reading file: " + fileName, ex);
            }

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/Xml/WCFCall");
            if (xmlNodeList != null)
            {
                foreach (XmlNode node in xmlNodeList)
                {
                    WcfCall wcfCall = WcfCall.LoadFromXml(node);
                    wcfCall.Source = fileName;
                    wcfCalls.Add(wcfCall);
                }
            }
            return wcfCalls;
        }

    }
}