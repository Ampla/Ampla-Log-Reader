using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Wcf
{
    public class WcfFaultSummaryReport : Report<WcfCall>
    {
        public WcfFaultSummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base(entries, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<WcfCall, WcfSummaryStatistic, string> analysis = new GroupByAnalysis<WcfCall, WcfSummaryStatistic, string>
                {
                    WhereFunc = entry => entry.IsFault,
                    GroupByFunc = entry => entry.FaultMessage,
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new WcfSummaryStatistic(key)
                };

            foreach (var wcfCall in Entries)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Fault Summary"))
            {
                List<WcfSummaryStatistic> summaryStatistics = analysis.Sort(WcfSummaryStatistic.CompareByCountDesc());
                if (summaryStatistics.Count > 0)
                {
                    reportWriter.Write("Fault Message");
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