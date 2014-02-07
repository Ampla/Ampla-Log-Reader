using System.Collections.Generic;
using System.Diagnostics;
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
            GroupByAnalysis<EventLogEntry, CountStatistic<EventLogEntry>> analysis = new GroupByAnalysis<EventLogEntry, CountStatistic<EventLogEntry>>
            {
                WhereFunc = entry => true,
                GroupByFunc = entry => "All Events",
                //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                StatisticFactory = key => new CountStatistic<EventLogEntry>(key)
            };

            foreach (var entry in Entries)
            {
                analysis.Add(entry);
            }

            using (reportWriter.StartReport(eventLog.LogDisplayName + " - Summary"))
            {
                reportWriter.Write("Name");
                foreach (Result result in analysis.Results)
                {
                    reportWriter.Write(result.Topic);
                }
                using (reportWriter.StartSection("Summary"))
                {
                    foreach (var result in analysis.Results)
                    {
                        reportWriter.Write(result);
                    }
                }
            }
        }
    }
}