using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public class TimeBasedStatistic<TEntry> : IStatistic<TEntry>
    {
        private readonly Func<TEntry, DateTime> dateTimeUtcFunc;
        private long firstEntryTicks;
        private long lastEntryTicks;

        public TimeBasedStatistic(string name, Func<TEntry, DateTime> dateTimeUtcFunc)
        {
            Name = name;
            this.dateTimeUtcFunc = dateTimeUtcFunc;
        }

        public void Add(TEntry entry)
        {
            DateTime dateTime = dateTimeUtcFunc(entry);
            long ticks = dateTime.Ticks;
            firstEntryTicks = Math.Min(firstEntryTicks, ticks);
            lastEntryTicks = Math.Max(lastEntryTicks, ticks);
        }

        public string Name { get; private set; }

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


        public IEnumerable<Result> Results
        {
            get
            {
                yield return Result.New<DateTime>(Name, "First Entry", FirstEntry);
                yield return Result.New<DateTime>(Name, "Last Entry", LastEntry);
            }
        }
    }
}