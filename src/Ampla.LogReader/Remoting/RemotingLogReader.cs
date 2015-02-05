using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingLogReader : LogReader<RemotingEntry>
    {
        private readonly string fileName;

        public RemotingLogReader(string fileName)
        {
            this.fileName = fileName;
            Name = new FileInfo(fileName).Name;
        }

        protected override List<RemotingEntry> ReadEntries()
        {
            List<RemotingEntry> entries = new List<RemotingEntry>();

            Exception ex;
            XmlDocument xmlDoc;

            xmlDoc = LoadXmlDocument(fileName,
                                     s => new SkipToContentTextReader("<RemotingEntry>", s), 
                                     out ex);
            if (xmlDoc == null)
            {
                xmlDoc = LoadXmlDocument(fileName,
                                         s => new TruncatedTextReader("</RemotingEntry>",
                                                  new SkipToContentTextReader("<RemotingEntry>", s)),
                                         out ex);
            }

            if (xmlDoc == null)
            {
                throw new XmlException("Unable to read file: " + fileName, ex);
            }

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/Xml/RemotingEntry");
            if (xmlNodeList != null)
            {
                entries.AddRange(from XmlNode node in xmlNodeList select RemotingEntry.LoadFromXml(node));
            }
            return entries;
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