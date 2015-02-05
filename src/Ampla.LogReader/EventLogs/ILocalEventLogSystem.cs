using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public interface ILocalEventLogSystem : IEventLogSystem
    {
        EventLog GetEventLog(string displayName);
        IEnumerable<EventLog> GetEventLogs();
    }
}