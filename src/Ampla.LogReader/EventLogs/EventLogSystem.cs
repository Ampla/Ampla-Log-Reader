using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;

namespace Ampla.LogReader.EventLogs
{
    public class EventLogSystem : IEventLogSystem
    {
        private readonly string machineName;

        public EventLogSystem() : this(".")
        {
        }

        private EventLogSystem(string machineName)
        {
            this.machineName = machineName;
        }

        public IEnumerable<EventLogReader> GetReaders()
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