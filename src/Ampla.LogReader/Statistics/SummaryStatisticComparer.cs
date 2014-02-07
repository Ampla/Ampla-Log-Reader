using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class SummaryStatisticComparer : IComparer<SummaryStatistic>
    {
        private readonly Func<SummaryStatistic, SummaryStatistic, int> compare;

        public SummaryStatisticComparer(Func<SummaryStatistic, SummaryStatistic, int> compare)
        {
            this.compare = compare;
        }

        public int Compare(SummaryStatistic x, SummaryStatistic y)
        {
            return compare(x, y);
        }
    }
}