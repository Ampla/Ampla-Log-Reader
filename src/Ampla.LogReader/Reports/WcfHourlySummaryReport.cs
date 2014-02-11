using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfHourlySummaryReport : Report<WcfCall>
    {
        public WcfHourlySummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base(entries, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<WcfCall,SummaryStatistic> analysis = new GroupByAnalysis<WcfCall,SummaryStatistic>
                {
                    WhereFunc = entry => true,
                    GroupByFunc = entry => entry.CallTime.ToString("yyyy-MM-dd HH-00Z"),
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in Entries)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Hourly Summary"))
            {
                List<SummaryStatistic> summaryStatistics = analysis.Sort(Statistic.CompareByName());
                if (summaryStatistics.Count > 0)
                {
                    reportWriter.Write("Hour");
                    foreach (Result result in summaryStatistics[0].Results)
                    {
                        reportWriter.Write(result.Topic);
                    }
                    foreach (SummaryStatistic summary in summaryStatistics)
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