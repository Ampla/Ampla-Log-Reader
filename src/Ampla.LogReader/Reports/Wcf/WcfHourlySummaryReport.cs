using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Wcf
{
    public class WcfHourlySummaryReport : Report<WcfCall>
    {
        public WcfHourlySummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base(entries, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<WcfCall, WcfSummaryStatistic> analysis = new GroupByAnalysis<WcfCall, WcfSummaryStatistic>
                {
                    WhereFunc = entry => true,
                    GroupByFunc = entry => entry.CallTime.ToString("yyyy-MM-dd HH-00Z"),
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new WcfSummaryStatistic(key)
                };

            foreach (var wcfCall in Entries)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Hourly Summary"))
            {
                List<WcfSummaryStatistic> summaryStatistics = analysis.Sort(Statistic.CompareByName());
                if (summaryStatistics.Count > 0)
                {
                    reportWriter.Write("Hour");
                    foreach (Result result in summaryStatistics[0].Results)
                    {
                        reportWriter.Write(result.Topic);
                    }
                    foreach (WcfSummaryStatistic summary in summaryStatistics)
                    {
                        using (reportWriter.StartSection(summary.Name))
                        {
                            foreach (Result result in summary.Results)
                            {
                                reportWriter.Write(result);
                            }
                        }
                    }
                }
            }
        }
    }
}