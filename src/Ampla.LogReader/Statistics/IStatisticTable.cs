using System.Data;

namespace Ampla.LogReader.Statistics
{
    public interface IStatisticTable<in TEntry>
    {
        void Add(TEntry entry);
        DataTable GetData();
        string Name { get; }
    }
}