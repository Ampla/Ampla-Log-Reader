using System.Collections.Generic;

namespace Ampla.LogReader.EventLogs
{
    public interface IEventLogSystem
    {
        IEnumerable<ILogReader<SimpleEventLogEntry>> GetReaders();
    }
}