﻿using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfSummaryReport : Report<WcfCall>
    {
        public WcfSummaryReport(List<WcfCall> entries, IReportWriter writer)
            : base(entries, writer)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<WcfCall, SummaryStatistic> analysis = new GroupByAnalysis<WcfCall, SummaryStatistic>
                {
                    WhereFunc = entry => true,
                    GroupByFunc = entry => "WCF Recorder",
                    //entry.Method, //entry => entry.CallTime.ToLocalTime().ToShortDateString(),
                    StatisticFactory = key => new SummaryStatistic(key)
                };

            foreach (var wcfCall in Entries)
            {
                analysis.Add(wcfCall);
            }

            using (reportWriter.StartReport("Wcf Summary"))
            {
                reportWriter.Write("Name");
                foreach (Result result in analysis.Results)
                {
                    reportWriter.Write(result.Topic);
                }
                using (reportWriter.StartSection("WCF Summary"))
                {
                    foreach (var result in analysis.Results)
                    {
                        reportWriter.Write(result);
                    }
                }
            }
        }

    }
}