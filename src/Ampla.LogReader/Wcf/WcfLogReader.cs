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
            Exception ex;
            XmlDocument xmlDoc;

            xmlDoc = LoadXmlDocument(fileName,
                                     s => new SkipToContentTextReader("<WCFCall>", s),
                                     out ex);
            if (xmlDoc == null)
            {
                xmlDoc = LoadXmlDocument(fileName,
                                         s => new TruncatedTextReader("</WCFCall>",
                                            new SkipToContentTextReader("<WCFCall>", s)),
                                         out ex);
            }

            if (xmlDoc == null)
            {
                throw new XmlException("Unable to read file: " + fileName, ex);
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

        private XmlDocument LoadXmlDocument(string xmlFileName, Func<Stream, TextReader> createTextReader, out Exception exception)
        {
            exception = null;
            using (FileStream stream = File.Open(xmlFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (TextReader contentReader = createTextReader(stream))
                {
                    using (TextReader reader = new XmlFragmentTextReader("Xml", contentReader))
                    {
                        try
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(reader);
                            return xmlDoc;
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                    }
                }
            }
            return null;
        }

    }
}