using System.Collections.Generic;

namespace Ampla.LogReader.Statistics
{
    public interface IStatistic<in TEntry>
    {
        void Add(TEntry entry);
        IEnumerable<Result> Results { get; }

        string Name { get; }
    }
}