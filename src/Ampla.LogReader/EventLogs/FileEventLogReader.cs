using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;

namespace Ampla.LogReader.EventLogs
{
    public class FileEventLogReader : LogReader<SimpleEventLogEntry>
    {
        private readonly EventLogQuery eventLogQuery;
        private readonly int limitEntries;

        public FileEventLogReader(string fileName, int limitEntries = -1)
        {
            eventLogQuery = new EventLogQuery(fileName, PathType.FilePath);
            Name = File.Exists(fileName) ? new FileInfo(fileName).Name : "EventLog (" + fileName + ")";
            this.limitEntries = limitEntries;
        }

        private static string RemoveDirectoryNames(string logName)
        {
            // sometimes the EventLog.LogName will include full filenames
            if (logName.Contains(@"\"))
            {
                if (File.Exists(logName))
                {
                    return new FileInfo(logName).Name;
                }
                return logName.Replace("\\", "_");
            }
            return logName;
        }

        protected override List<SimpleEventLogEntry> ReadEntries()
        {
            List<SimpleEventLogEntry> entries = new List<SimpleEventLogEntry>();
            using (System.Diagnostics.Eventing.Reader.EventLogReader reader = new System.Diagnostics.Eventing.Reader.EventLogReader(eventLogQuery))
            {
                using (ThreadCulture.SetUICulture("en-US"))
                {
                    EventRecord eventRecord = reader.ReadEvent();
                    int count = 0;
                    if (eventRecord != null)
                    {
                        // Use the name of event log from the records
                        // may need to remove directory names
                        Name = RemoveDirectoryNames(eventRecord.LogName);
                        count = 1;
                    }
                    if (limitEntries > 0)
                    {
                        while (eventRecord != null && count <= limitEntries)
                        {
                            entries.Add(new SimpleEventLogEntry(eventRecord));
                            eventRecord = reader.ReadEvent();
                            count++;
                        }
                    }
                    else
                    {
                        while (eventRecord != null)
                        {
                            entries.Add(new SimpleEventLogEntry(eventRecord));
                            eventRecord = reader.ReadEvent();
                        }
                    }
                    return entries;
                }
            }
        }
    }
}