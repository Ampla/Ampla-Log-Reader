using System.Collections;

namespace Ampla.LogReader.Statistics
{
    public interface IStatistic<in TEntry>
    {
        void Add(TEntry entry);
        IEnumerable Results { get; }
    }
}