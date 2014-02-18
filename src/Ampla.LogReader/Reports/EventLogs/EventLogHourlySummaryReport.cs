using System.Collections.Generic;
using System.Globalization;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.EventLogs.Statistics;
using Ampla.LogReader.ReportWriters;

namespace Ampla.LogReader.Reports.EventLogs
{
    public class EventLogHourlySummaryReport : HourlySummaryReport<SimpleEventLogEntry, EventLogEntryTypeStatistic>
    {
        public EventLogHourlySummaryReport(string reportName, List<SimpleEventLogEntry> entries, IReportWriter writer) :
            base(reportName,
                 entries,
                 writer,
                 entry => entry.CallTime,
                 key => new EventLogEntryTypeStatistic(key.ToString(CultureInfo.InvariantCulture)))
        {
        }
    }
}