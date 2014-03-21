using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogSystem
    {
        IEnumerable<EventLogReader> GetReaders();

        EventLog GetEventLog(string displayName);
        IEnumerable<EventLog> GetEventLogs();
    }
}