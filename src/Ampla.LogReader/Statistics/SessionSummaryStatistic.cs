using System.Collections.Generic;
using Ampla.LogReader.Remoting;

namespace Ampla.LogReader.Statistics
{
    
    public class SessionSummaryStatistic : IStatistic<RemotingEntry>
    {
        private readonly GroupByAnalysis<RemotingEntry, TimeBasedStatistic<RemotingEntry, IdentitySession>, IdentitySession> groupByAnalysis;
        
        public SessionSummaryStatistic(string name)
        {
            Name = name;
            groupByAnalysis = new GroupByAnalysis<RemotingEntry, TimeBasedStatistic<RemotingEntry, IdentitySession>, IdentitySession>
                {
                    GroupByFunc = entry => new IdentitySession(entry.Identity, entry.Arguments[0].Value),
                    WhereFunc = entry => entry.Arguments.Length > 0 && entry.Arguments[0].TypeName == "System.Guid",
                    StatisticFactory = key => new TimeBasedStatistic<RemotingEntry, IdentitySession>(key, entry => entry.CallTimeLocal, true)
                };
        }

        public void Add(RemotingEntry entry)
        {
            groupByAnalysis.Add(entry);
        }

        public List<TimeBasedStatistic<RemotingEntry, IdentitySession>> Sessions
        {
            get
            {
                return groupByAnalysis.Sort();
            }
        }

        public IEnumerable<Result> Results
        {
            get
            {
                List<Result> results = new List<Result>();
                var sessions = Sessions;
                foreach (var session in sessions)
                {
                    results.AddRange(session.Results);
                }
                return results;
            }
        }

        public int Count { get { return groupByAnalysis.Count; } }

        public string Name { get; private set; }

    }
}