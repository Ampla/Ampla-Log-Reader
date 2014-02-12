using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public class EventLogReader : ILogReader<EventLogEntry>
    {
        private readonly EventLog eventLog;
        private readonly List<EventLogEntry> entries;

        public EventLogReader(EventLog eventLog)
        {
            this.eventLog = eventLog;
            entries = new List<EventLogEntry>();
        }

        public void Read()
        {
            foreach (EventLogEntry entry in eventLog.Entries)
            {
                entries.Add(entry);
            }
        }

        public List<EventLogEntry> Entries
        {
            get { return entries; }
        }
    }
}