using System.Collections.Generic;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Statistics
{
    /// <summary>
    ///     Count the number of Entries
    /// </summary>
    public class CountStatistic<TEntry> : IStatistic<TEntry>
    {
        private readonly string name;
        private int count;

        public CountStatistic(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void Add(TEntry entry)
        {
            count++;
        }

        public IEnumerable<Result> Results
        {
            get 
            { 
                yield return Result.New(name, "Count", count); 
            }
        }

        public override string ToString()
        {
            return string.Format("{0} -> Count: {1}", name, count);
        }

    }
}