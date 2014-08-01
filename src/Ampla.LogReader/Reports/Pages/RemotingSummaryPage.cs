using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.Pages
{
    public class RemotingSummaryPage : ReportPage<RemotingEntry>
    {
        private readonly RemotingSummaryStatistic summaryStatistic;
        private readonly TopNStatistics<RemotingEntry> top50IdentityStats;
        private readonly TopNStatistics<RemotingEntry> top50MethodStats;

        public RemotingSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<RemotingEntry> entries)
            : base(excelSpreadsheet, entries, "Summary")
        {
            summaryStatistic = new RemotingSummaryStatistic("Summary");
            top50IdentityStats = new TopNStatistics<RemotingEntry>
                ("Top 50 Identities", 50,
                 entry => entry.Identity,
                 entry => true);

            top50MethodStats = new TopNStatistics<RemotingEntry>
                ("Top 50 Methods", 50,
                 entry => entry.Method,
                 entry => true);
        }

        protected override void RenderPage(IWorksheetWriter writer)
        {
            UpdateStatistics(new IStatistic<RemotingEntry>[] {summaryStatistic, top50IdentityStats, top50MethodStats});

            writer.WriteRow("Summary of Remoting calls");
            if (summaryStatistic.Count > 0)
            {
                writer.WriteRow("Count: ", summaryStatistic.Count);
                writer.WriteRow("From: ", summaryStatistic.FirstEntry.ToLocalTime());
                writer.WriteRow("To: ", summaryStatistic.LastEntry.ToLocalTime());
                writer.WriteRow("Duration (hrs): ", summaryStatistic.LastEntry.Subtract(summaryStatistic.FirstEntry).TotalHours);

                var current = writer.GetCurrentCell();
                writer.WriteRow();

                WriteStatistics(top50MethodStats, writer);

                writer.MoveTo(current.Row, current.Column + 3);
                writer.WriteRow();

                WriteStatistics(top50IdentityStats, writer);
            }
            else
            {
                writer.WriteRow("Count:", "no entries");
            }
        }

        
    }
}