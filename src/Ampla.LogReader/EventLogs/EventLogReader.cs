using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ampla.LogReader.EventLogs
{
    public class EventLogReader : LogReader<SimpleEventLogEntry>
    {
        private readonly EventLog eventLog;

        public EventLogReader(EventLog eventLog)
        {
            this.eventLog = eventLog;
            Name = eventLog.LogDisplayName;
        }

        protected override List<SimpleEventLogEntry> ReadEntries()
        {
            return (from EventLogEntry entry in eventLog.Entries select new SimpleEventLogEntry(entry)).ToList();
        }

        public string Name { get; private set; }

    }
}