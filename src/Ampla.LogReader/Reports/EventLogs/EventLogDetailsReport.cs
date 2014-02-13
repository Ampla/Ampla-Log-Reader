using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.ReportWriters;

namespace Ampla.LogReader.Reports.EventLogs
{
    public class EventLogDetailsReport : Report<EventLogEntry>
    {
        private readonly EventLog eventLog;

        public EventLogDetailsReport(EventLog eventLog, List<EventLogEntry> entries, IReportWriter reportWriter)
            : base(entries, reportWriter)
        {
            this.eventLog = eventLog;
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            if (Entries.Count > 0)
            {
                using (reportWriter.StartReport(eventLog.LogDisplayName + "-Dump"))
                {
                    reportWriter.Write("Index");
                    reportWriter.Write("TimeGenerated");
                    reportWriter.Write("Source");
                    reportWriter.Write("InstanceId");
                    reportWriter.Write("Category");
                    reportWriter.Write("EntryType");
                    reportWriter.Write("MachineName");
                    reportWriter.Write("UserName");
                    reportWriter.Write("Message");

                    foreach (EventLogEntry entry in Entries)
                    {
                        string instance = string.Format("{0}", entry.Index);
                        using (reportWriter.StartSection(instance))
                        {
                            reportWriter.Write("{0}", entry.TimeGenerated);
                            reportWriter.Write("{0}", entry.Source);
                            reportWriter.Write("{0}", entry.InstanceId);
                            reportWriter.Write("{0}", entry.Category);
                            reportWriter.Write("{0}", entry.EntryType);
                            reportWriter.Write("{0}", entry.MachineName);
                            reportWriter.Write("{0}", entry.UserName);
                            reportWriter.Write("{0}", entry.Message);
                        }
                    }
                }
            }
        }
    }
}