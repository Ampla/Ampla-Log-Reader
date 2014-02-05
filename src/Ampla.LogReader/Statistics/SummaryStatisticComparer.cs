using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class SummaryStatisticComparer : IComparer<SummaryStatistic>
    {
        private readonly Func<SummaryStatistic, SummaryStatistic, int> comparer;

        public SummaryStatisticComparer(Func<SummaryStatistic, SummaryStatistic, int> comparer)
        {
            this.comparer = comparer;
        }

        public int Compare(SummaryStatistic x, SummaryStatistic y)
        {
            return comparer(x, y);
        }
    }
}