using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class TimeBasedStatistic<TEntry, TKey> : IStatistic<TEntry>, IComparable<TimeBasedStatistic<TEntry, TKey>> where TKey :IComparable<TKey> 
    {
        private readonly Func<TEntry, DateTime> dateTimeUtcFunc;
        private long firstEntryTicks = DateTime.MaxValue.Ticks;
        private long lastEntryTicks = DateTime.MinValue.Ticks;
        private int count;

        public TimeBasedStatistic(TKey key, Func<TEntry, DateTime> dateTimeUtcFunc)
        {
            Name = key.ToString();
            Key = key;
            this.dateTimeUtcFunc = dateTimeUtcFunc;
        }

        public void Add(TEntry entry)
        {
            DateTime dateTime = dateTimeUtcFunc(entry);
            long ticks = dateTime.Ticks;
            count++;
            firstEntryTicks = Math.Min(firstEntryTicks, ticks);
            lastEntryTicks = Math.Max(lastEntryTicks, ticks);
        }

        public string Name { get; private set; }

        public TKey Key { get; private set; }

        public DateTime? FirstEntry
        {
            get { return count == 0 ? (DateTime?) null : new DateTime(firstEntryTicks, DateTimeKind.Utc).ToLocalTime(); }
        }

        public DateTime? LastEntry
        {
            get { return count == 0 ? (DateTime?) null : new DateTime(lastEntryTicks, DateTimeKind.Utc).ToLocalTime(); }
        }

        public TimeSpan Duration
        {
            get
            {
                long durationTicks = lastEntryTicks - firstEntryTicks;
                return durationTicks > 0 ? TimeSpan.FromTicks(durationTicks) : TimeSpan.Zero;
            }
        }

        public int Count
        {
            get { return count; }
        }

        public IEnumerable<Result> Results
        {
            get
            {
                yield return Result.New<DateTime?>(Name, "First Entry", FirstEntry);
                yield return Result.New<DateTime?>(Name, "Last Entry", LastEntry);
                yield return Result.New<int>(Name, "Count", Count);
            }
        }

        public int CompareTo(TimeBasedStatistic<TEntry, TKey> other)
        {
            int compare = firstEntryTicks.CompareTo(other.firstEntryTicks);
            if (compare == 0)
            {
                compare = -lastEntryTicks.CompareTo(other.lastEntryTicks);
            }
            if (compare == 0)
            {
                compare = -count.CompareTo(other.count);
            }
            if (compare == 0)
            {
                compare = Key.CompareTo(other.Key);
            }

            return compare;
        }

        public override string ToString()
        {
            if (count > 0)
            {
                return string.Format("TimeBasedStatistic<{0}> '{1}' (Count: {2}) (Start: {3}) (End: {4})",
                                     typeof(TEntry).Name, Name, count, FirstEntry, LastEntry);

            }
            return string.Format("TimeBasedStatistic<{0}> '{1}' (Count: 0)", typeof(TEntry).Name, Name);
        }
    }
}