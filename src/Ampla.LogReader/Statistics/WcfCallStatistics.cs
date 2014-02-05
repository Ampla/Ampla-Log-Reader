using System;
using System.Collections.Generic;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Statistics
{
    public class WcfCallAnalysis<TStatistic> : IWcfStatistic where TStatistic : IWcfStatistic
    {
        private Func<WcfCall, string> selector = (entry) => entry.Url;
        private Func<WcfCall, bool> filterFunc = (entry) => true;
        private Func<string, TStatistic> factory;

        private readonly Dictionary<string, TStatistic> dictionary = new Dictionary<string, TStatistic>();

        public Func<WcfCall, bool> FilterFunc
        {
            set
            {
                filterFunc = value;
            }
        }

        public Func<WcfCall, string> SelectFunc
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

        public void Add(WcfCall entry)
        {
            if (filterFunc(entry))
            {
                string key = selector.Invoke(entry);
                IWcfStatistic statistic = GetStatisticForKey(key);
                statistic.Add(entry);
            }
        }

        protected IWcfStatistic GetStatisticForKey(string key)
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