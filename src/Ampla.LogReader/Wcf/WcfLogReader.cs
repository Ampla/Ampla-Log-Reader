using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Wcf
{
    public class WcfLogReader : IWcfLogReader
    {
        private readonly string fileName;
        private readonly List<WcfCall> wcfCalls = new List<WcfCall>();

        public WcfLogReader(string fileName)
        {
            this.fileName = fileName;
        }

        public void Read()
        {
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
                    wcfCalls.Add(WcfCall.LoadFromXml(node));
                }
            }
        }

        public List<WcfCall> WcfCalls
        {
            get { return wcfCalls; }
        }
    }
}