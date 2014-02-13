using System.Collections.Generic;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.Remoting
{
    public class RemotingIdentitySummaryReport : Report<RemotingEntry>
    {
        public RemotingIdentitySummaryReport(List<RemotingEntry> entries, IReportWriter reportWriter)
            : base(entries, reportWriter)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<RemotingEntry, RemotingSummaryStatistic, string> analysis = new GroupByAnalysis<RemotingEntry, RemotingSummaryStatistic, string>
            {
                WhereFunc = entry => true,
                GroupByFunc = entry => entry.Identity,
                //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                StatisticFactory = key => new RemotingSummaryStatistic(key)
            };

            foreach (var entry in Entries)
            {
                analysis.Add(entry);
            }

            var summaries = analysis.Sort(RemotingSummaryStatistic.CompareByCountDesc());

            if (summaries.Count > 0)
            {
                using (reportWriter.StartReport("Remoting - Identity"))
                {
                    reportWriter.Write("Identity");
                    foreach (Result result in summaries[0].Results)
                    {
                        reportWriter.Write(result.Topic);
                    }
                    foreach (var summary in summaries)
                    {
                        using (reportWriter.StartSection(summary.Name))
                        {
                            foreach (var result in summary.Results)
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