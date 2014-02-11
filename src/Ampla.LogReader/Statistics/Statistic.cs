using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public abstract class Statistic
    {
        private readonly string name;

        protected Statistic(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public static IComparer<Statistic> CompareByName()
        {
            return new Comparer<Statistic>((x, y) => StringComparer.InvariantCulture.Compare(x.Name, y.Name));
        }
        
    }

    public abstract class Statistic<TEntry> : Statistic, IStatistic<TEntry>
    {
        protected Statistic(string name) : base(name)
        {
        }

        public abstract void Add(TEntry entry);

        public abstract IEnumerable<Result> Results { get; }
    }
}