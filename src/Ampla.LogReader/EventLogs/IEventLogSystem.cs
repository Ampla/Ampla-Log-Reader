using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogSystem
    {
        EventLog[] GetEventLogs();

        EventLog GetEventLog(string displayName);
    }
}