using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ampla.LogReader.EventLogs
{
    public class EventLogReader : LogReader<SimpleEventLogEntry>
    {
        private readonly EventLog eventLog;
        private readonly int limitEntries;

        public EventLogReader(EventLog eventLog, int limitEntries = -1)
        {
            this.eventLog = eventLog;
            Name = eventLog.LogDisplayName;
            this.limitEntries = limitEntries;
        }

        protected override List<SimpleEventLogEntry> ReadEntries()
        {
            if (limitEntries > 0)
            {
                List<SimpleEventLogEntry> entries = new List<SimpleEventLogEntry>();

                int number = Math.Min(eventLog.Entries.Count, limitEntries);
                for (int i = 0; i < number; i++)
                {   
                    entries.Add(new SimpleEventLogEntry(eventLog.Entries[i]));
                }
                return entries;
            }
            
            return (from EventLogEntry entry in eventLog.Entries select new SimpleEventLogEntry(entry)).ToList();
        }
    }
}