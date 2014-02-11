using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.EventLogs.Statistics;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.EventLogs
{
    public class EventLogHourlySummaryReport : Report<EventLogEntry>
    {
        private readonly EventLog eventLog;

        public EventLogHourlySummaryReport(EventLog eventLog, List<EventLogEntry> entries, IReportWriter writer)
            : base(entries, writer)
        {
            this.eventLog = eventLog;
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<EventLogEntry, EventLogEntryTypeStatistic> analysis = new GroupByAnalysis<EventLogEntry, EventLogEntryTypeStatistic>
            {
                WhereFunc = entry => true,
                GroupByFunc = entry => entry.TimeGenerated.ToString("yyyy-MM-dd HH-00Z"),
                StatisticFactory = key => new EventLogEntryTypeStatistic(key)
            };

            foreach (var entry in Entries)
            {
                analysis.Add(entry);
            }

            string reportName = eventLog.LogDisplayName + "-Hourly Summary";

            if (reportName.Length > 31)
            {
                reportName = eventLog.LogDisplayName.Length > 31 ? reportName.Substring(1, 31) : eventLog.LogDisplayName;
            }

            var summaries = analysis.Sort(Statistic.CompareByName());

            if (summaries.Count > 0)
            {
                using (reportWriter.StartReport(reportName))
                {
                    reportWriter.Write("Category");
                    foreach (Result result in summaries[0].Results)
                    {
                        reportWriter.Write(result.Topic);
                    }
                    foreach (var summary in summaries)
                    {
                        using (reportWriter.StartSection(summary.Name))
                        {
                            foreach (var result in summary.Results)
                            {
                                reportWriter.Write(result);
                            }
                        }
                    }
                }
            }
        }
    }
}