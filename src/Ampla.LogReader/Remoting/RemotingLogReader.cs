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
        }

        protected override List<RemotingEntry> ReadEntries()
        {
            List<RemotingEntry> entries = new List<RemotingEntry>();
            XmlContentTextReader contentReader = new XmlContentTextReader("<RemotingEntry>", File.OpenRead(fileName));
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

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/Xml/RemotingEntry");
            if (xmlNodeList != null)
            {
                entries.AddRange(from XmlNode node in xmlNodeList select RemotingEntry.LoadFromXml(node));
            }
            return entries;
        }
    }
}