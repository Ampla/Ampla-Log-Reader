using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.EventLogs.Statistics;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.EventLogs
{
    public class EventLogSummaryReport : Report<EventLogEntry>
    {
        private readonly EventLog eventLog;

        public EventLogSummaryReport(EventLog eventLog, List<EventLogEntry> entries, IReportWriter reportWriter) : base(entries, reportWriter)
        {
            this.eventLog = eventLog;
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<EventLogEntry, EventLogEntryTypeStatistic> analysis = new GroupByAnalysis<EventLogEntry, EventLogEntryTypeStatistic>
            {
                WhereFunc = entry => true,
                GroupByFunc = entry => entry.Source,
                //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                StatisticFactory = key => new EventLogEntryTypeStatistic(key)
            };

            foreach (var entry in Entries)
            {
                analysis.Add(entry);
            }

            string reportName = eventLog.LogDisplayName + " - Summary";

            if (reportName.Length > 31)
            {
                reportName = eventLog.LogDisplayName.Length > 31 ? reportName.Substring(1, 31) : eventLog.LogDisplayName;
            }

            var summaries = analysis.Sort(new Statistics.Comparer<EventLogEntryTypeStatistic>(EventLogEntryTypeStatistic.Comparer));

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