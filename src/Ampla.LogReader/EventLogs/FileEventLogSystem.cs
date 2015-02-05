using System.Collections.Generic;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public class FileEventLogSystem : IEventLogSystem
    {
        private readonly string fileName;

        public FileEventLogSystem(string fileName)
        {
            this.fileName = fileName;
        }
        public IEnumerable<ILogReader<SimpleEventLogEntry>> GetReaders()
        {
            List<ILogReader<SimpleEventLogEntry>> list = new List<ILogReader<SimpleEventLogEntry>>
                {
                    new EventFileLogReader(fileName)
                };
            return list;
        }

     }
}