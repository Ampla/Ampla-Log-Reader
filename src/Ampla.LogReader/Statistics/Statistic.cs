using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public abstract class Statistic<TEntry> : IStatistic<TEntry>
    {
        private readonly string name;

        protected Statistic(string name)
        {
            this.name = name;
        }

        public abstract void Add(TEntry entry);

        public abstract IEnumerable<Result> Results { get; }

        public string Name
        {
            get { return name; }
        }

    }
}