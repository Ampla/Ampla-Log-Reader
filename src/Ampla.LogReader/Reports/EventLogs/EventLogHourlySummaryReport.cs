using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.EventLogs.Statistics;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.EventLogs
{
    public class EventLogHourlySummaryReport : HourlySummaryReport<EventLogEntry, EventLogEntryTypeStatistic>
    {
        public EventLogHourlySummaryReport(string reportName, List<EventLogEntry> entries, IReportWriter writer) :
            base(reportName,
                 entries,
                 writer,
                 entry => entry.TimeGenerated,
                 key => new EventLogEntryTypeStatistic(key.ToString()))
        {
        }
    }
}