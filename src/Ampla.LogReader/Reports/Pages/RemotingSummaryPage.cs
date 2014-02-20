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
        private readonly TopNStatistics<RemotingEntry> top10IdentityStats;
        private readonly TopNStatistics<RemotingEntry> top10MethodStats;

        public RemotingSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<RemotingEntry> entries)
            : base(excelSpreadsheet, entries, "Summary")
        {
            summaryStatistic = new RemotingSummaryStatistic("Summary");
            top10IdentityStats = new TopNStatistics<RemotingEntry>
                ("Top 20 Identities", 20,
                 entry => entry.Identity,
                 entry => true);

            top10MethodStats = new TopNStatistics<RemotingEntry>
                ("Top 20 Methods", 20,
                 entry => entry.Method,
                 entry => true);
        }

        protected override void RenderPage(IWorksheetWriter writer)
        {
            UpdateStatistics(new IStatistic<RemotingEntry>[] {summaryStatistic, top10IdentityStats, top10MethodStats});

            writer.WriteRow("Summary of Remoting calls");
            if (summaryStatistic.Count > 0)
            {
                writer.WriteRow("Count: ", summaryStatistic.Count);
                writer.WriteRow("From: ", summaryStatistic.FirstEntry.ToLocalTime());
                writer.WriteRow("To: ", summaryStatistic.LastEntry.ToLocalTime());
                writer.WriteRow("Duration (hrs): ", summaryStatistic.LastEntry.Subtract(summaryStatistic.FirstEntry).TotalHours);

                var current = writer.GetCurrentCell();
                writer.WriteRow();
                
                writer.WriteRow(top10IdentityStats.Name, "Count");
                foreach (var result in top10IdentityStats.Results)
                {
                    writer.WriteRow(result.Topic, (IConvertible) result.Data);
                }

                writer.MoveTo(current.Row, current.Column + 3);
                writer.WriteRow();

                writer.WriteRow(top10MethodStats.Name, "Count");
                foreach (var result in top10MethodStats.Results)
                {
                    writer.WriteRow(result.Topic, (IConvertible)result.Data);
                }
            }
            else
            {
                writer.WriteRow("Count:", "no entries");
            }
        }
        
    }
}