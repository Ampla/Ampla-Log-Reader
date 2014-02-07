using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogReader : ILogReader
    {
        List<EventLogEntry> EventLogEntries { get; }
    }
}