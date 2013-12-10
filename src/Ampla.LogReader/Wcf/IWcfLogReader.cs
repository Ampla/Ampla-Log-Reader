using System.Collections.Generic;

namespace Ampla.LogReader.Wcf
{
    public interface IWcfLogReader : ILogReader
    {
        List<WcfCall> WcfCalls { get; }
    }
}