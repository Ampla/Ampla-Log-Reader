using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class Comparer<TStatistic> : IComparer<TStatistic> where TStatistic : Statistic
    {
        private readonly Func<TStatistic, TStatistic, int> compare;

        public Comparer(Func<TStatistic, TStatistic, int> compare)
        {
            this.compare = compare;
        }

        public int Compare(TStatistic x, TStatistic y)
        {
            return compare(x, y);
        }
    }
}