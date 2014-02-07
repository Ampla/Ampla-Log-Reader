using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfActionSummaryReport : Report<WcfCall>
    {
        public WcfActionSummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base(entries, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            WcfCallAnalysis<SummaryStatistic> analysis = new WcfCallAnalysis<SummaryStatistic>
                {
                    FilterFunc = entry => true,
                    SelectFunc = entry => entry.Action,
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in Entries)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Action Summary"))
            {
                List<SummaryStatistic> summaries = analysis.Sort(SummaryStatistic.CompareCountDesc());

                if (summaries.Count > 0)
                {
                    reportWriter.Write("Action");
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