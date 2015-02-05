using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace Ampla.LogReader.EventLogs
{
    public class EventFileLogReader : LogReader<SimpleEventLogEntry>
    {
        private readonly EventLogQuery eventLogQuery;

        public EventFileLogReader(string fileName)
        {
            eventLogQuery = new EventLogQuery(fileName, PathType.FilePath);
            Name = File.Exists(fileName) ? new FileInfo(fileName).Name : "EventLog (" + fileName + ")";
        }

        protected override List<SimpleEventLogEntry> ReadEntries()
        {
            List<SimpleEventLogEntry> entries = new List<SimpleEventLogEntry>();
            using (System.Diagnostics.Eventing.Reader.EventLogReader reader = 
                    new System.Diagnostics.Eventing.Reader.EventLogReader(eventLogQuery))
            {
               EventRecord eventRecord = reader.ReadEvent();
                if (eventRecord != null)
                {
                    Name = eventRecord.LogName;
                }
                while (eventRecord != null)
                {
                    entries.Add(new SimpleEventLogEntry(eventRecord));
                }
                return entries;
            }
        }
    }
}