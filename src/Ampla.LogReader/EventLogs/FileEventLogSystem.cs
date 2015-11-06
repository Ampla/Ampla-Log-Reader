using System.Collections.Generic;
using System.IO;

namespace Ampla.LogReader.EventLogs
{
    public class FileEventLogSystem : IEventLogSystem
    {
        private readonly string[] fileNames;
        private int limitEntries;

        public FileEventLogSystem(params string[] fileNames) : this(-1, fileNames)
        {
        }

        public FileEventLogSystem(int limitEntries, params string[] fileNames)
        {
            this.fileNames = fileNames;
            this.limitEntries = limitEntries;
        }

        public IEnumerable<ILogReader<SimpleEventLogEntry>> GetReaders()
        {
            List<ILogReader<SimpleEventLogEntry>> list = new List<ILogReader<SimpleEventLogEntry>>();
            foreach (string fileName in fileNames)
            {
                if (File.Exists(fileName))
                {
                    list.Add(new FileEventLogReader(fileName, limitEntries));
                }
            }
            return list;
        }

     }
}