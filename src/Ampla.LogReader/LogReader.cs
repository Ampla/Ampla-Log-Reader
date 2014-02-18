using System.Collections.Generic;

namespace Ampla.LogReader
{
    public abstract class LogReader<TEntry> : ILogReader<TEntry>
    {
        protected LogReader()
        {
            Entries = new List<TEntry>();
        }

        private bool hasRead;

        public void Read()
        {
            if (!hasRead)
            {
                Entries = ReadEntries();
            }
            hasRead = true;
        }

        protected abstract List<TEntry> ReadEntries();

        public List<TEntry> Entries { get; private set; }
    }
}