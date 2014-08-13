using System.Collections.Generic;
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

            writer.WriteRow("Sessions");
            if (sessionStatistics.Count > 0)
            {
                writer.WriteRow("Identity", "Session", "Count", "First", "Last", "Hours");
                var sessions = sessionStatistics.Sessions;
                foreach (var session in sessions)
                {
                    writer.WriteRow(session.Key.Identity, session.Key.Session, session.Count, session.FirstEntry, session.LastEntry, session.Duration.TotalHours);
                }
            }
            else
            {
                writer.WriteRow("None");
            }

        }
        
    }
}