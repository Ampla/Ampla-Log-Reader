using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogSystem
    {
        EventLogReader[] GetReaders();

        EventLog GetEventLog(string displayName);
    }
}