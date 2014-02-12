using System.Collections.Generic;

namespace Ampla.LogReader
{
    public interface ILogReader<TEntry>
    {
        void Read();

        List<TEntry> Entries { get; }
    }
}