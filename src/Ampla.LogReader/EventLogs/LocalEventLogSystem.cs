using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

namespace Ampla.LogReader.EventLogs
{
    public class LocalEventLogSystem : ILocalEventLogSystem
    {
        private readonly string machineName;

        public LocalEventLogSystem() : this(".")
        {
        }

        private LocalEventLogSystem(string machineName)
        {
            this.machineName = machineName;
        }

        public IEnumerable<ILogReader<SimpleEventLogEntry>> GetReaders()
        {
            return GetEventLogs().Select(eventLog => new EventLogReader(eventLog)).ToArray();
        }

        public IEnumerable<EventLog> GetEventLogs()
        {
            List<EventLog> eventLogs = new List<EventLog>();
            foreach (var eventLog in EventLog.GetEventLogs(machineName))
            {
                try
                {
                    if (eventLog.LogDisplayName != null)
                    {
                        eventLogs.Add(eventLog);
                    }
                }
                catch (SecurityException)
                {
                }
            }

            return eventLogs;
        }

        public EventLog GetEventLog(string displayName)
        {
            return GetEventLogs().FirstOrDefault(eventLog => eventLog.LogDisplayName == displayName);
        }

    }
}