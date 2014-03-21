using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.EventLogs.Statistics
{
    public class EventLogSummaryTable : IStatisticTable<EventLogReader>
    {
        private readonly DataTable dataTable;

        public EventLogSummaryTable(string name)
        {
            Name = name;
            dataTable = new DataTable(Name);
            dataTable.Columns.Add("EventLog", typeof (string));
            dataTable.Columns.Add("Errors", typeof(int));
            dataTable.Columns.Add("Warnings", typeof(int));
            dataTable.Columns.Add("Infos", typeof(int));
            dataTable.Columns.Add("Total", typeof(int));
        }

        public void Add(EventLogReader entry)
        {
            entry.Read();
            EventLogEntryTypeStatistic statistic = new EventLogEntryTypeStatistic(entry.Name);
            foreach (SimpleEventLogEntry logEntry in entry.Entries)
            {
                statistic.Add(logEntry);
            }
            dataTable.Rows.Add(entry.Name, statistic.ErrorCount, statistic.WarningCount, statistic.InformationCount, statistic.TotalCount);
        }

        public DataTable GetData()
        {
            return dataTable;
        }

        public string Name { get; private set; }

    }
}