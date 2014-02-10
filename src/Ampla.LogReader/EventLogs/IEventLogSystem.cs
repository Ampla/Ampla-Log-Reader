using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogSystem
    {
        IList<EventLog> GetEventLogs();

        EventLog GetEventLog(string displayName);
    }
}