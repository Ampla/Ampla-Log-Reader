using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.Remoting
{
    public class RemotingSummaryPage : ReportPage<RemotingEntry>
    {
        private readonly RemotingSummaryStatistic summaryStatistic;
        private readonly TopNStatistics<RemotingEntry> top10IdentityStats;

        public RemotingSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<RemotingEntry> entries)
            : base(excelSpreadsheet, entries, "Summary")
        {
            summaryStatistic = new RemotingSummaryStatistic("Summary");
            top10IdentityStats = new TopNStatistics<RemotingEntry>
                ("Top 3 Identities", 3,
                 entry => entry.Identity,
                 entry => true);
        }

        protected override void RenderPage(IWorksheetWriter writer, IEnumerable<RemotingEntry> entries)
        {
            UpdateStatistics(new IStatistic<RemotingEntry>[] {summaryStatistic, top10IdentityStats});

            writer.WriteRow("Summary of Remoting calls");
            if (summaryStatistic.Count > 0)
            {
                writer.WriteRow("Count: ", summaryStatistic.Count);
                writer.WriteRow("From: ", summaryStatistic.FirstEntry.ToLocalTime());
                writer.WriteRow("To: ", summaryStatistic.LastEntry.ToLocalTime());
                writer.WriteRow("Duration (hrs): ", summaryStatistic.LastEntry.Subtract(summaryStatistic.FirstEntry).TotalHours);
                
                writer.WriteRow();
                
                writer.WriteRow("Top 3 Identities");
                foreach (var result in top10IdentityStats.Results)
                {
                    writer.WriteRow(result.Topic, (IConvertible) result.Data);
                }
            }
            else
            {
                writer.WriteRow("Count:", "no entries");
            }

            

        }
        
    }
}