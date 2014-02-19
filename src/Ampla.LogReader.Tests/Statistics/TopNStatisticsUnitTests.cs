using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Statistics
{
    [TestFixture]
    public class TopNStatisticsUnitTests : TestFixture
    {
        private class TestEntry
        {
            public TestEntry(string group)
            {
                Group = group;
            }
            public string Group { get; private set; }
        }

        [Test]
        public void Top1Stats()
        {
            TopNStatistics<TestEntry> topStats = new TopNStatistics<TestEntry>("Top 1", 1, 
                entry => entry.Group, 
                entry => true);

            Assert.That(topStats.Results, Is.Empty);

            topStats.Add(new TestEntry("One"));

            List<Result> results = new List<Result>(topStats.Results);

            Assert.That(results.Count, Is.EqualTo(1));
            topStats.Add(new TestEntry("Two"));

            results = new List<Result>(topStats.Results);
            Assert.That(results.Count, Is.EqualTo(1));
        }

        [Test]
        public void Top10Stats()
        {
            TopNStatistics<TestEntry> topStats = new TopNStatistics<TestEntry>("Top 10", 10,
                entry => entry.Group,
                entry => true);

            Assert.That(topStats.Results, Is.Empty);

            topStats.Add(new TestEntry("One"));

            List<Result> results = new List<Result>(topStats.Results);

            Assert.That(results.Count, Is.EqualTo(1));
            topStats.Add(new TestEntry("Two"));

            results = new List<Result>(topStats.Results);
            Assert.That(results.Count, Is.EqualTo(2));
        }
 
    }
}