using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Statistics
{
    [TestFixture]
    public class CountStatisticUnitTests : TestFixture
    {
        public class Entry
        {
        }

        [Test]
        public void EmptyStatistic()
        {
            CountStatistic<Entry> statistic = new CountStatistic<Entry>("UnitTest");
            Assert.That(statistic.Count, Is.EqualTo(0));
            Assert.That(statistic.Results, Is.Not.Empty);

            List<Result> results = new List<Result>();
            results.AddRange(statistic.Results);

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Name, Is.EqualTo("UnitTest"));
            Assert.That(results[0].Topic, Is.EqualTo("Count"));
            Assert.That(results[0].Data, Is.EqualTo(0));
        }

        [Test]
        public void OneEntry()
        {
            CountStatistic<Entry> statistic = new CountStatistic<Entry>("UnitTest");
            statistic.Add(new Entry());
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.Results, Is.Not.Empty);

            List<Result> results = new List<Result>();
            results.AddRange(statistic.Results);

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].Name, Is.EqualTo("UnitTest"));
            Assert.That(results[0].Topic, Is.EqualTo("Count"));
            Assert.That(results[0].Data, Is.EqualTo(1));
        }

    }
}