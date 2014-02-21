using System;
using System.Collections.Generic;
using System.Linq;

namespace Ampla.LogReader.Statistics
{
    /// <summary>
    ///     A set of summary statistics 
    /// </summary>
    public class SummaryStatistic<TEntry> : Statistic<TEntry>
    {
        private readonly Func<TEntry, bool> isFaultFunc;
        private readonly Func<TEntry, DateTime> callTimeFunc;
        private readonly Func<TEntry, TimeSpan> durationFunc;
        private long firstEntryTicks = DateTime.MaxValue.Ticks;
        private long lastEntryTicks = DateTime.MinValue.Ticks;
        private TimeSpan totalDuration = TimeSpan.Zero;
        private TimeSpan maxDuration = TimeSpan.Zero;

        protected SummaryStatistic(string name, Func<TEntry, bool> isFaultFunc, Func<TEntry, DateTime> callTimeFunc, Func<TEntry, TimeSpan> durationFunc) : base(name)
        {
            this.isFaultFunc = isFaultFunc;
            this.callTimeFunc = callTimeFunc;
            this.durationFunc = durationFunc;
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public override void Add(TEntry entry)
        {
            Count++;
            if (isFaultFunc != null)
            {
                if (isFaultFunc(entry))
                {
                    Errors++;
                }
            }
            if (callTimeFunc != null)
            {
                DateTime callTime = callTimeFunc(entry);
                long ticks = callTime.Ticks;
                firstEntryTicks = Math.Min(firstEntryTicks, ticks);
                lastEntryTicks = Math.Max(lastEntryTicks, ticks);
            }
            if (durationFunc != null)
            {
                TimeSpan duration = durationFunc(entry);
                totalDuration = totalDuration.Add(duration);
                maxDuration = duration > maxDuration ? duration : maxDuration;
            }
        }

        public override IEnumerable<Result> Results
        {
            get
            {
                yield return Result.New<int>(Name, "Count", Count);
                if (callTimeFunc != null)
                {
                    yield return Result.New<DateTime>(Name, "First Entry", FirstEntry);
                    yield return Result.New<DateTime>(Name, "Last Entry", LastEntry);
                }
                if (isFaultFunc != null)
                {
                    yield return Result.New<int>(Name, "Errors", Errors);
                    yield return Result.New<double>(Name, "Percentage", ErrorPercent);
                }
                if (durationFunc != null)
                {
                    yield return Result.New<TimeSpan>(Name, "Total Duration", totalDuration);
                    yield return Result.New<TimeSpan>(Name, "Maximum Duration", maxDuration);
                    yield return Result.New<TimeSpan>(Name, "Average Duration", new TimeSpan(totalDuration.Ticks/Count));
                }
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

        public TimeSpan TotalDuration  
        {
            get
            {
                if (durationFunc == null)
                {
                    throw new InvalidOperationException("Unable to Calculate Total Duration");
                }
                return totalDuration;
            }
        }

        public TimeSpan MaxDuration
        {
            get
            {
                if (durationFunc == null)
                {
                    throw new InvalidOperationException("Unable to Calculate Maximum Duration");
                }
                return maxDuration;
            }
        }

        public TimeSpan AverageDuration
        {
            get
            {
                if (durationFunc == null)
                {
                    throw new InvalidOperationException("Unable to Calculate Average Duration");
                }

                int count = Count == 0 ? 1 : Count;

                return new TimeSpan(totalDuration.Ticks / count);
            }
        }


        public override string ToString()
        {
            List<string> values = Results.Select(result => result.ToString()).ToList();
            return Name + "\n\t" + string.Join("\n\t", values);
        }

        public static IComparer<SummaryStatistic<TEntry>> CompareByCountDesc()
        {
            return new Comparer<SummaryStatistic<TEntry>>((x, y) => y.Count.CompareTo(x.Count));
        }
    }
}