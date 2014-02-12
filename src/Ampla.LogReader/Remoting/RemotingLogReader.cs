using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ampla.LogReader.Wcf;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingLogReader : ILogReader<RemotingEntry>
    {
        private readonly string fileName;
        private readonly List<RemotingEntry> entries = new List<RemotingEntry>();

        public RemotingLogReader(string fileName)
        {
            this.fileName = fileName;
        }

        public void Read()
        {
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
                foreach (XmlNode node in xmlNodeList)
                {
                    entries.Add(RemotingEntry.LoadFromXml(node));
                }
            }
        }

        public List<RemotingEntry> Entries
        {
            get { return entries; }
        }
    }
}