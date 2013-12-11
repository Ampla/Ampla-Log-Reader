using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Render
{
    public class WcfErrorStatisticsReport : Report
    {
        public WcfErrorStatisticsReport(List<WcfCall> wcfCalls, TextWriter writer) : base(wcfCalls, writer)
        {
        }

        protected override void RenderReport(TextWriter textWriter)
        {
            WcfCallStatistics<SummaryStatistic> statistic = new WcfCallStatistics<SummaryStatistic>
                {
                    FilterFunc = entry => true,
                    SelectFunc = entry => "WCF Recorder", //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };
            
            foreach (var wcfCall in WcfCalls)
            {
                statistic.Add(wcfCall);
            }

            foreach (var result in statistic.Results)
            {
                textWriter.WriteLine(result);
            }
        }
    }
}