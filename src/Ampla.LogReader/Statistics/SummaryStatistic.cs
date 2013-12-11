using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Statistics
{
    /// <summary>
    ///     A set of summary statistics 
    /// </summary>
    public class SummaryStatistic : IWcfStatistic
    {
        private readonly string name;
        private int errors;
        private int count ;
        private long firstEntryTicks = DateTime.MaxValue.Ticks;
        private long lastEntryTicks = DateTime.MinValue.Ticks;

        public SummaryStatistic(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void Add(WcfCall entry)
        {
            if (entry.IsFault)
            {
                errors++;
            }
            count++;
            long ticks = entry.CallTime.Ticks;
            firstEntryTicks = Math.Min(firstEntryTicks, ticks);
            lastEntryTicks = Math.Max(lastEntryTicks, ticks);
        }

        public IEnumerable Results
        {
            get
            {
                yield return "Count: " + count; 
                yield return string.Format("First Entry: {0:dd-MMM-yyyy HH:mm:ss}", FirstEntry);
                yield return string.Format("Last Entry: {0:dd-MMM-yyyy HH:mm:ss}", LastEntry);
                yield return "Errors: " + errors;
                yield return "Percentage: " + ErrorPercent.ToString("0.00");
            }
        }

        public double ErrorPercent
        {
            get { return count == 0 ? double.NaN : errors*100.0D/count; }
        }

        public DateTime FirstEntry
        {
            get
            {
                return new DateTime(firstEntryTicks, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public DateTime LastEntry
        {
            get
            {
                return new DateTime(lastEntryTicks, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public override string ToString()
        {
            List<string> values = Results.Cast<string>().ToList();

            return name + "\n\t" + string.Join("\n\t", values);
        }

    }
}