using System.Collections.Generic;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using NUnit.Framework;

namespace Ampla.LogReader.Statistics
{
    [TestFixture]
    public class SessionSummaryStatisticUnitTests : TestFixture
    {
        private LogReader<RemotingEntry> reader;

        protected override void OnFixtureSetUp()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();
            reader = new RemotingDirectory(project);
            reader.Read();
            Assert.That(reader.Entries, Is.Not.Empty);
            base.OnFixtureSetUp();
        }

        [Test]
        public void EmptyStatistic()
        {
            SessionSummaryStatistic statistic = new SessionSummaryStatistic("Sessions");
            Assert.That(statistic.Count, Is.EqualTo(0));
            Assert.That(statistic.Results, Is.Empty);
        }

        [Test]
        public void OneEntry()
        {
            SessionSummaryStatistic statistic = new SessionSummaryStatistic("Sessions");
            RemotingEntry entry = reader.Entries.Find(match => match.Arguments.Length > 0);
            statistic.Add(entry);
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.Results, Is.Not.Empty);
            List<TimeBasedStatistic<RemotingEntry, IdentitySession>> sessions = statistic.Sessions;

            Assert.That(sessions, Is.Not.Empty);
            Assert.That(sessions.Count, Is.EqualTo(1));

            Assert.That(sessions[0].Count, Is.EqualTo(1));
        }

        [Test]
        public void AddTwoEntries()
        {
            SessionSummaryStatistic statistic = new SessionSummaryStatistic("Sessions");
            RemotingEntry entry = reader.Entries.Find(match => match.Arguments.Length > 0);
            statistic.Add(entry);
            RemotingEntry entry2 = reader.Entries.Find(match => match.ArgumentXml != entry.ArgumentXml);
            statistic.Add(entry2);
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.Results, Is.Not.Empty);
        }
    }
}