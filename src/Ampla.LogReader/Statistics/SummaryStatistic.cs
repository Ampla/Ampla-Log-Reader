using System;
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
        private int errors;
        private int count ;
        private long firstEntryTicks = DateTime.MaxValue.Ticks;
        private long lastEntryTicks = DateTime.MinValue.Ticks;
        private TimeSpan totalDuration = TimeSpan.Zero;
        private TimeSpan maxDuration = TimeSpan.Zero;

        public SummaryStatistic(string name)
        {
            Name = name;
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
            totalDuration = totalDuration.Add(entry.Duration);
            maxDuration = entry.Duration > maxDuration ? entry.Duration : maxDuration;
        }

        public IEnumerable<Result> Results
        {
            get
            {
                yield return Result.New<int>(Name, "Count", Count);
                yield return Result.New<DateTime>(Name, "First Entry", FirstEntry);
                yield return Result.New<DateTime>(Name, "Last Entry", LastEntry);
                yield return Result.New<int>(Name, "Errors", Errors);
                yield return Result.New<double>(Name, "Percentage", ErrorPercent);
                yield return Result.New<TimeSpan>(Name, "Total Duration", totalDuration);
                yield return Result.New<TimeSpan>(Name, "Maximum Duration", maxDuration);
                yield return Result.New<TimeSpan>(Name, "Average Duration", new TimeSpan(totalDuration.Ticks / count));
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

        public int Count
        {
            get { return count; }
        }

        public int Errors
        {
            get { return errors; }
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            List<string> values = Results.Select(result => result.ToString()).ToList();
            return Name + "\n\t" + string.Join("\n\t", values);
        }

        public static IComparer<SummaryStatistic> CompareName()
        {
            return new SummaryStatisticComparer((x, y) => StringComparer.InvariantCulture.Compare(x.Name, y.Name));
        }

        public static IComparer<SummaryStatistic> CompareDate()
        {
            return new SummaryStatisticComparer((x, y) => x.firstEntryTicks.CompareTo(y.firstEntryTicks));
        }

        public static IComparer<SummaryStatistic> CompareCountDesc()
        {
            return new SummaryStatisticComparer((x, y) => y.count.CompareTo(x.count));
        }
    }
}