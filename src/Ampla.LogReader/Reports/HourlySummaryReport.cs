using System;
using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports
{
    public class HourlySummaryReport<TEntry, TStatistic> : Report<TEntry> where TStatistic : IStatistic<TEntry>
    {
        private readonly string reportName;
        private readonly Func<TEntry, DateTime> callTimeFunc;
        private readonly Func<DateTime, TStatistic> statisticFunc;

        protected HourlySummaryReport(string reportName, List<TEntry> entries, IReportWriter writer, Func<TEntry, DateTime> callTimeFunc, Func<DateTime, TStatistic> statisticFunc )
            : base(entries, writer)
        {
            this.reportName = reportName;
            this.callTimeFunc = callTimeFunc;
            this.statisticFunc = statisticFunc;
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            GroupByAnalysis<TEntry, TStatistic, DateTime> analysis = new GroupByAnalysis<TEntry, TStatistic, DateTime>
            {
                WhereFunc = entry => true,
                GroupByFunc = entry => RoundToHour(callTimeFunc(entry)),
                StatisticFactory = key => statisticFunc(key),
            };

            foreach (var entry in Entries)
            {
                analysis.Add(entry);
            }

            var summaries = analysis.Sort((IComparer<TStatistic>) Statistic.CompareByName());

            if (summaries.Count > 0)
            {
                using (reportWriter.StartReport(reportName))
                {
                    reportWriter.Write("Category");
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
        private DateTime RoundToHour(DateTime localTime)
        {
            return new DateTime(localTime.Year, localTime.Month, localTime.Day, localTime.Hour, 0, 0, DateTimeKind.Local);
        }
    }
}