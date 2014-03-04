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
                ("Top Urls", 100,
                 entry => entry.Url,
                 entry => true);

            top10MethodStats = new TopNStatistics<WcfCall>
                ("Top Methods", 100,
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
                writer.WriteRow();
                writer.WriteRow("Number of Faults:", summaryStatistic.Errors);
                writer.WriteRow("Fault (%):", summaryStatistic.ErrorPercent);
                writer.WriteRow();
                writer.WriteRow("Total Duration (sec): ", summaryStatistic.TotalDuration.TotalSeconds);
                writer.WriteRow("Maximum Duration (sec): ", summaryStatistic.MaxDuration.TotalSeconds);
                writer.WriteRow("Average Duration (sec): ", summaryStatistic.AverageDuration.TotalSeconds);

                var current = writer.GetCurrentCell();
                writer.WriteRow();

                WriteStatistics(top10MethodStats, writer);

                writer.MoveTo(current.Row, current.Column + 3);
                writer.WriteRow();

                WriteStatistics(top10UrlStats, writer);
            }
            else
            {
                writer.WriteRow("Count:", "no entries");
            }
        }
    }
}