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
    public class SummaryStatistic : Statistic<WcfCall>
    {
        private long firstEntryTicks = DateTime.MaxValue.Ticks;
        private long lastEntryTicks = DateTime.MinValue.Ticks;
        private TimeSpan totalDuration = TimeSpan.Zero;
        private TimeSpan maxDuration = TimeSpan.Zero;

        public SummaryStatistic(string name) : base(name)
        {
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public override void Add(WcfCall entry)
        {
            if (entry.IsFault)
            {
                Errors++;
            }
            Count++;
            long ticks = entry.CallTime.Ticks;
            firstEntryTicks = Math.Min(firstEntryTicks, ticks);
            lastEntryTicks = Math.Max(lastEntryTicks, ticks);
            totalDuration = totalDuration.Add(entry.Duration);
            maxDuration = entry.Duration > maxDuration ? entry.Duration : maxDuration;
        }

        public override IEnumerable<Result> Results
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
                yield return Result.New<TimeSpan>(Name, "Average Duration", new TimeSpan(totalDuration.Ticks / Count));
            }
        }

        public double ErrorPercent
        {
            get { return Count == 0 ? 0.0d : Errors*100.0D/Count; }
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

        public int Count { get; private set; }

        public int Errors { get; private set; }

        public override string ToString()
        {
            List<string> values = Results.Select(result => result.ToString()).ToList();
            return Name + "\n\t" + string.Join("\n\t", values);
        }

        public static IComparer<SummaryStatistic> CompareCountDesc()
        {
            return new Comparer<SummaryStatistic>((x, y) => y.Count.CompareTo(x.Count));
        }
    }
}