using System;
using System.Data;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.EventLogs.Statistics
{
    public class EventLogSummaryTable : IStatisticTable<ILogReader<SimpleEventLogEntry>>
    {
        private readonly DataTable dataTable;
        private readonly DateTime last1Hr;
        private readonly DateTime last24Hrs;

        public EventLogSummaryTable(string name)
        {
            DateTime nowUtc = DateTime.UtcNow;
            last1Hr = nowUtc.AddHours(-1);
            last24Hrs = nowUtc.AddDays(-1);
            Name = name;
            dataTable = new DataTable(Name);
            dataTable.Columns.Add("EventLog", typeof (string));
            dataTable.Columns.Add("Error-1hr", typeof(int));
            dataTable.Columns.Add("Warning-1hr", typeof(int));
            dataTable.Columns.Add("Information-1hr", typeof(int));
            dataTable.Columns.Add("Error-24hrs", typeof(int));
            dataTable.Columns.Add("Warning-24hrs", typeof(int));
            dataTable.Columns.Add("Information-24hrs", typeof(int));
            dataTable.Columns.Add("Error", typeof(int));
            dataTable.Columns.Add("Warning", typeof(int));
            dataTable.Columns.Add("Information", typeof(int));
            dataTable.Columns.Add("Total", typeof(int));
        }

        public void Add(ILogReader<SimpleEventLogEntry> entry)
        {
            entry.ReadAll();
            EventLogEntryTypeStatistic statistic = new EventLogEntryTypeStatistic(entry.Name);
            EventLogEntryTypeStatistic lastHour = new EventLogEntryTypeStatistic(entry.Name);
            EventLogEntryTypeStatistic lastDay = new EventLogEntryTypeStatistic(entry.Name);
            foreach (SimpleEventLogEntry logEntry in entry.Entries)
            {
                if (logEntry.CallTime >= last24Hrs)
                {
                    lastDay.Add(logEntry);
                    if (logEntry.CallTime >= last1Hr)
                    {
                        lastHour.Add(logEntry);
                    }
                }
                statistic.Add(logEntry);
            }
            dataTable.Rows.Add(entry.Name, 
                lastHour.ErrorCount, lastHour.WarningCount, lastHour.InformationCount,
                lastDay.ErrorCount, lastDay.WarningCount, lastDay.InformationCount,
                statistic.ErrorCount, statistic.WarningCount, statistic.InformationCount, statistic.TotalCount);
        }

        public DataTable GetData()
        {
            return dataTable;
        }

        public string Name { get; private set; }

    }
}