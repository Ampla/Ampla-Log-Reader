using System.Collections.Generic;

namespace Ampla.LogReader
{
    public interface ILogReader<TEntry>
    {
        void ReadAll();
        List<TEntry> Entries { get; }
        string Name { get; }
    }
}