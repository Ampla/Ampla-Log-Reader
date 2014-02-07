using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfFaultSummaryReport : Report
    {
        public WcfFaultSummaryReport(List<WcfCall> wcfCalls, IReportWriter writer)
            : base(wcfCalls, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            WcfCallAnalysis<SummaryStatistic> analysis = new WcfCallAnalysis<SummaryStatistic>
                {
                    FilterFunc = entry => entry.IsFault,
                    SelectFunc = entry => entry.FaultMessage,
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in WcfCalls)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Fault Summary"))
            {
                List<SummaryStatistic> summaryStatistics = analysis.Sort(SummaryStatistic.CompareCountDesc());
                if (summaryStatistics.Count > 0)
                {
                    reportWriter.Write("Fault Message");
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