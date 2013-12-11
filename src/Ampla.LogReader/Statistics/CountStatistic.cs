using System.Collections;
using System.Collections.Generic;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Statistics
{
    /// <summary>
    ///     Count the number of WcfCalls
    /// </summary>
    public class CountStatistic : IWcfStatistic
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
        public void Add(WcfCall entry)
        {
            count++;
        }

        public IEnumerable Results
        {
            get
            {
                yield return "Count: " + count;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} -> Count: {1}", name, count);
        }

    }
}