using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Render
{
    public class WcfSummaryReport : Report
    {
        public WcfSummaryReport(List<WcfCall> wcfCalls, IReportWriter writer)
            : base(wcfCalls, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            WcfCallAnalysis<SummaryStatistic> analysis = new WcfCallAnalysis<SummaryStatistic>
                {
                    FilterFunc = entry => true,
                    SelectFunc = entry => "WCF Recorder",
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in WcfCalls)
            {
                analysis.Add(wcfCall);
            }

            foreach (var result in analysis.Results)
            {
                reportWriter.Write(result);
            }
        }

    }
}