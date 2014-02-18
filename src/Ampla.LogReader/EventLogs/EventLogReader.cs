using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public class EventLogReader : ILogReader<SimpleEventLogEntry>
    {
        private readonly EventLog eventLog;
        private readonly List<SimpleEventLogEntry> entries;
        private bool hasRead;

        public EventLogReader(EventLog eventLog)
        {
            this.eventLog = eventLog;
            entries = new List<SimpleEventLogEntry>();
            Name = eventLog.LogDisplayName;
        }

        public void Read()
        {
            if (!hasRead)
            {
                foreach (EventLogEntry entry in eventLog.Entries)
                {
                    entries.Add(new SimpleEventLogEntry(entry));
                }
            }
            hasRead = true;
        }

        public string Name { get; private set; }

        public List<SimpleEventLogEntry> Entries
        {
            get { return entries; }
        }
    }
}