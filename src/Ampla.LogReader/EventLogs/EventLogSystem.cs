using System;
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

        public EventLog[] GetEventLogs()
        {
            return EventLog.GetEventLogs(machineName);
        }

        public EventLog GetEventLog(string displayName)
        {
            foreach (var eventLog in GetEventLogs())
            {
                try
                {
                    if (eventLog.LogDisplayName == displayName)
                    {
                        return eventLog;
                    }
                }
                catch (SecurityException)
                {
                }
                
            }
            return null;
        }
    }
}