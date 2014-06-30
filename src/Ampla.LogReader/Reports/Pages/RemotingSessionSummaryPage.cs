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
        
        public RemotingSessionSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<RemotingEntry> entries)
            : base(excelSpreadsheet, entries, "Sessions")
        {
            sessionStatistics = new SessionSummaryStatistic("Sessions");
        }

        protected override void RenderPage(IWorksheetWriter writer)
        {
            UpdateStatistic(sessionStatistics);

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