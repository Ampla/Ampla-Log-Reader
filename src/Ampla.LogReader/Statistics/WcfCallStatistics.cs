using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Statistics
{
    public class WcfCallStatistics<TStatistic> : IWcfStatistic where TStatistic : IWcfStatistic
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

        public IEnumerable Results
        {
            get
            {
                return new List<TStatistic>(dictionary.Values);
            }
        }
    }
}