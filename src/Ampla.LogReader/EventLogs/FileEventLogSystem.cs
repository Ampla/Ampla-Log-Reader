using System.Collections.Generic;
using System.IO;

namespace Ampla.LogReader.EventLogs
{
    public class FileEventLogSystem : IEventLogSystem
    {
        private readonly string[] fileNames;

        public FileEventLogSystem(params string[] fileNames)
        {
            this.fileNames = fileNames;
        }

        public IEnumerable<ILogReader<SimpleEventLogEntry>> GetReaders()
        {
            List<ILogReader<SimpleEventLogEntry>> list = new List<ILogReader<SimpleEventLogEntry>>();
            foreach (string fileName in fileNames)
            {
                if (File.Exists(fileName))
                {
                    list.Add(new FileEventLogReader(fileName));
                }
            }
            return list;
        }

     }
}