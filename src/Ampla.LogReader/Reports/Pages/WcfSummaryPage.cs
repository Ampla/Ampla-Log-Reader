using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Pages
{
    public class WcfSummaryPage : ReportPage<WcfCall>
    {
        private readonly WcfSummaryStatistic summaryStatistic;
        private readonly TopNStatistics<WcfCall> top10UrlStats;
        private readonly TopNStatistics<WcfCall> top10MethodStats;

        public WcfSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<WcfCall> entries)
            : base(excelSpreadsheet, entries, "Summary")
        {
            summaryStatistic = new WcfSummaryStatistic("Summary");
            top10UrlStats = new TopNStatistics<WcfCall>
                ("Top 20 Urls", 20,
                 entry => entry.Url,
                 entry => true);

            top10MethodStats = new TopNStatistics<WcfCall>
                ("Top 20 Methods", 20,
                 entry => entry.Method,
                 entry => true);
        }

        protected override void RenderPage(IWorksheetWriter writer)
        {
            UpdateStatistics(new IStatistic<WcfCall>[] {summaryStatistic, top10UrlStats, top10MethodStats});

            writer.WriteRow("Summary of Wcf calls");
            if (summaryStatistic.Count > 0)
            {
                writer.WriteRow("Count: ", summaryStatistic.Count);
                writer.WriteRow("From: ", summaryStatistic.FirstEntry.ToLocalTime());
                writer.WriteRow("To: ", summaryStatistic.LastEntry.ToLocalTime());
                writer.WriteRow("Duration (hrs): ", summaryStatistic.LastEntry.Subtract(summaryStatistic.FirstEntry).TotalHours);

                var current = writer.GetCurrentCell();
                writer.WriteRow();
                
                writer.WriteRow(top10UrlStats.Name, "Count");
                foreach (var result in top10UrlStats.Results)
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