using System;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Statistics
{
    public class WcfSummaryStatistic : SummaryStatistic<WcfCall>
    {
        public WcfSummaryStatistic(string name) : base(name, 
            wcf => wcf.IsFault, 
            wcf => wcf.CallTime, 
            wcf => wcf.Duration)
        {
        }
    }
}