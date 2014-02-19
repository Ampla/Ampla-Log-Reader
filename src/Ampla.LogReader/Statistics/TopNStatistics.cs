using System;
using System.Collections.Generic;
using System.Linq;

namespace Ampla.LogReader.Statistics
{
    public class TopNStatistics<TEntry> : IStatistic<TEntry>
    {
        private readonly int topN;
        private readonly GroupByAnalysis<TEntry, CountStatistic<TEntry>, string> groupByAnalysis;

        public TopNStatistics(string name, int topN, Func<TEntry, string> groupByFunc, Func<TEntry, bool> whereFunc)
        {
            Name = name;
            this.topN = topN;
            groupByAnalysis = new GroupByAnalysis<TEntry, CountStatistic<TEntry>, string>
                {
                    GroupByFunc = groupByFunc,
                    WhereFunc = whereFunc ?? (entry => true),
                    StatisticFactory = key => new CountStatistic<TEntry>(key)
                };
        }

        public void Add(TEntry entry)
        {
            groupByAnalysis.Add(entry);
        }

        public IEnumerable<Result> Results
        {
            get {
                var descCounts = groupByAnalysis.Sort(CountStatistic<TEntry>.CompareByCountDesc());

                int counter = 0;

                return (from count in descCounts where counter++ < topN select Result.New(Name, count.Name, count.Count)).ToList();
            }
        }

        public string Name { get; private set; }
    }
}