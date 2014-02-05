﻿using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Render
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

            reportWriter.NewSubject("Wcf Fault Summary");

            foreach (SummaryStatistic summary in analysis.Sort(SummaryStatistic.CompareCountDesc()))
            {
                reportWriter.NewSubject(summary.Name);
                foreach (Result result in summary.Results)
                {
                    reportWriter.Write(result);
                }
            }
        }

    }
}