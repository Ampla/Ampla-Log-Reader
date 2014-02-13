using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Wcf
{
    public class WcfHourlySummaryReport : HourlySummaryReport<WcfCall, WcfSummaryStatistic>
    {
        public WcfHourlySummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base("Wcf Hourly Summary", 
            entries, 
            writer, 
            entry => entry.CallTime,
            key => new WcfSummaryStatistic(key.ToString("yyyy-MM-dd HH:mmZ")))
        {
        }
    }
}