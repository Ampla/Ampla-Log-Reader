using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class GroupByAnalysis<TEntry, TStatistic, TKey> where TStatistic : IStatistic<TEntry>
    {
        private Func<TEntry, TKey> selector;
        private Func<TEntry, bool> filterFunc = (entry) => true;
        private Func<TKey, TStatistic> factory;

        private readonly Dictionary<TKey, TStatistic> dictionary = new Dictionary<TKey, TStatistic>();

        public Func<TEntry, bool> WhereFunc
        {
            set
            {
                filterFunc = value;
            }
        }

        public Func<TEntry, TKey> GroupByFunc
        {
            set
            {
                selector = value;
            }
        }

        public Func<TKey, TStatistic> StatisticFactory
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
                TKey key = selector.Invoke(entry);
                IStatistic<TEntry> statistic = GetStatisticForKey(key);
                statistic.Add(entry);
            }
        }

        private IStatistic<TEntry> GetStatisticForKey(TKey key)
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