﻿using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfActionSummaryReport : Report
    {
        public WcfActionSummaryReport(List<WcfCall> wcfCalls, IReportWriter writer)
            : base(wcfCalls, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            WcfCallAnalysis<SummaryStatistic> analysis = new WcfCallAnalysis<SummaryStatistic>
                {
                    FilterFunc = entry => true,
                    SelectFunc = entry => entry.Action,
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in WcfCalls)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Action Summary"))
            {
                foreach (SummaryStatistic summary in analysis.Sort(SummaryStatistic.CompareCountDesc()))
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