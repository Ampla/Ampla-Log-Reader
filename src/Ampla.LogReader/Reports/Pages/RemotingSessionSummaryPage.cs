using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.Pages
{
    public class RemotingSessionSummaryPage : ReportPage<RemotingEntry>
    {
        private readonly SessionSummaryStatistic sessionStatistics;
        private readonly TimeBasedStatistic<RemotingEntry, string> summary; 
        
        public RemotingSessionSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<RemotingEntry> entries)
            : base(excelSpreadsheet, entries, "Sessions")
        {
            sessionStatistics = new SessionSummaryStatistic("Sessions");
            summary = new TimeBasedStatistic<RemotingEntry, string>("TimeBase", entry => entry.CallTimeUtc);
        }

        protected override void RenderPage(IWorksheetWriter writer)
        {
            UpdateStatistics(new IStatistic<RemotingEntry>[] {sessionStatistics, summary});

            DateTime offset = summary.FirstEntry.HasValue ? summary.FirstEntry.Value : new DateTime(DateTime.Now.Year, 1, 1, 0,0,0,DateTimeKind.Local);
            offset = offset.TruncateToLocalDay();

            writer.WriteRow("Sessions");
            if (sessionStatistics.Count > 0)
            {
                writer.WriteRow("Identity", "Session", "Count", "First", "Last", "Hours", "Offset", "Duration");
                var sessions = sessionStatistics.Sessions;
                foreach (var session in sessions)
                {
                    Debug.Assert(session.FirstEntry != null, "session.FirstEntry != null");
                    DateTime firstEntry = session.FirstEntry.Value;
                    DateTime? lastEntry = session.LastEntry;
                    double start = firstEntry.Subtract(offset).TotalDays;
                    double duration = session.Duration.TotalDays; 
                    writer.WriteRow(session.Key.Identity, session.Key.Session, session.Count, 
                        firstEntry, lastEntry, 
                        session.Duration.TotalHours,
                        start, duration );
                }
            }
            else
            {
                writer.WriteRow("None");
            }

        }
        
    }
}