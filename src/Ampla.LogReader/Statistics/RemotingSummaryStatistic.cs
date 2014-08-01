using Ampla.LogReader.Remoting;

namespace Ampla.LogReader.Statistics
{
    public class RemotingSummaryStatistic : SummaryStatistic<RemotingEntry>
    {
        public RemotingSummaryStatistic(string name)
            : base(name, 
            null /* no faults */,
            entry => entry.CallTimeUtc, 
            entry => entry.Duration)
        {
        }
    }
}