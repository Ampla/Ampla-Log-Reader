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

        public EventLogSystem(string machineName)
        {
            this.machineName = machineName;
        }

        public IList<EventLog> GetEventLogs()
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