using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfUrlSummaryReport : Report<WcfCall>
    {
        public WcfUrlSummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base(entries, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<WcfCall, SummaryStatistic> analysis = new GroupByAnalysis<WcfCall, SummaryStatistic>
                {
                    WhereFunc = entry => true,
                    GroupByFunc = entry => entry.Url,
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in Entries)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Url Summary"))
            {
                List<SummaryStatistic> summaries = analysis.Sort(SummaryStatistic.CompareCountDesc());
                if (summaries.Count > 0)
                {
                    reportWriter.Write("Url");
                    foreach (Result result in summaries[0].Results)
                    {
                        reportWriter.Write(result.Topic);
                    }

                    foreach (SummaryStatistic summary in summaries)
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