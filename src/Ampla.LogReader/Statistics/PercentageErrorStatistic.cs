using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    /// <summary>
    ///     Determine the Percentage Error
    /// </summary>
    public class PercentageErrorStatistic<TEntry> : IStatistic<TEntry>
    {
        private readonly string name;
        private readonly Func<TEntry, bool> isErrorFunc;
        private int errors;
        private int total ;

        public PercentageErrorStatistic(string name, Func<TEntry, bool> isErrorFunc)
        {
            this.name = name;
            this.isErrorFunc = isErrorFunc;
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void Add(TEntry entry)
        {
            if (isErrorFunc(entry))
            {
                errors++;
            }
            total++;
        }

        public IEnumerable<Result> Results
        {
            get
            {
                yield return Result.New(name, "Errors", errors);
                yield return Result.New(name, "Total", total);
                yield return Result.New(name, "Percentage", ErrorPercent);
            }
        }

        public double ErrorPercent
        {
            get { return total == 0 ? double.NaN : errors*100.0D/total; }
        }

        public override string ToString()
        {
            return string.Format("{0}\nErrors ({1}) / Total ({2}) = {3:00.0}% errors", name, errors, total, ErrorPercent);
        }

    }
}