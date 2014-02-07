using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfUrlSummaryReport : Report
    {
        public WcfUrlSummaryReport(List<WcfCall> wcfCalls, IReportWriter writer)
            : base(wcfCalls, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            WcfCallAnalysis<SummaryStatistic> analysis = new WcfCallAnalysis<SummaryStatistic>
                {
                    FilterFunc = entry => true,
                    SelectFunc = entry => entry.Url,
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in WcfCalls)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Url Summary"))
            {
                List<SummaryStatistic> summaries = analysis.Sort(SummaryStatistic.CompareCountDesc());

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