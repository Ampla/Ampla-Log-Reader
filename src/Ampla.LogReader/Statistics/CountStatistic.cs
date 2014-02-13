using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    /// <summary>
    ///     Count the number of Entries
    /// </summary>
    public class CountStatistic<TEntry> : Statistic<TEntry>
    {
        private int count;

        public CountStatistic(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public override void Add(TEntry entry)
        {
            count++;
        }

        public override IEnumerable<Result> Results
        {
            get { yield return Result.New(Name, "Count", count); }
        }

        public int Count
        {
            get { return count; }
        }

        public override string ToString()
        {
            return string.Format("{0} -> Count: {1}", Name, count);
        }
    }
}