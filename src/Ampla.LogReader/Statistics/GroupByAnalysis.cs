using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class GroupByAnalysis<TEntry, TStatistic> where TStatistic : IStatistic<TEntry>
    {
        private Func<TEntry, string> selector;
        private Func<TEntry, bool> filterFunc = (entry) => true;
        private Func<string, TStatistic> factory;

        private readonly Dictionary<string, TStatistic> dictionary = new Dictionary<string, TStatistic>();

        public Func<TEntry, bool> WhereFunc
        {
            set
            {
                filterFunc = value;
            }
        }

        public Func<TEntry, string> GroupByFunc
        {
            set
            {
                selector = value;
            }
        }

        public Func<string, TStatistic> StatisticFactory
        {
            set
            {
                factory = value;
            }
        }

        public void Add(TEntry entry)
        {
            if (filterFunc(entry))
            {
                string key = selector.Invoke(entry);
                IStatistic<TEntry> statistic = GetStatisticForKey(key);
                statistic.Add(entry);
            }
        }

        protected IStatistic<TEntry> GetStatisticForKey(string key)
        {
            TStatistic statistic;
            if (!dictionary.TryGetValue(key, out statistic))
            {
                statistic = factory(key);
                dictionary[key] = statistic;
            }
            return statistic;
        }

        public List<TStatistic> Sort(IComparer<TStatistic> comparer)
        {
            List<TStatistic> list = new List<TStatistic>();
            list.AddRange(dictionary.Values);
            list.Sort(comparer);
            return list;
        }

        public IEnumerable<Result> Results
        {
            get
            {
                List<Result> results = new List<Result>();
                foreach (var statistic in dictionary)
                {
                    results.AddRange(statistic.Value.Results);
                }
                return results;
            }
        }
    }
}