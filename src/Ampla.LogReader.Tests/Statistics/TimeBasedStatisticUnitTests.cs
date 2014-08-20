using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Ampla.LogReader.Statistics
{
    [TestFixture]
    public class TimeBasedStatisticUnitTests : TestFixture
    {
        private class Entry
        {
            public Entry()
            {
                Utc = DateTime.UtcNow;
                Local = DateTime.Now;
            }

            public DateTime Utc { get; set; }
            public DateTime Local { get; set; }

            public static DateTime UtcFunc(Entry entry)
            {
                return entry.Utc;
            }

            public static DateTime LocalFunc(Entry entry)
            {
                return entry.Local;
            }
        }

        [Test]
        public void EmptyStatistic()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("UnitTest", Entry.UtcFunc);
            Assert.That(statistic.Count, Is.EqualTo(0));
            Assert.That(statistic.FirstEntry, Is.Null);
            Assert.That(statistic.LastEntry, Is.Null);
            Assert.That(statistic.Results, Is.Not.Empty);
        }

        [Test]
        public void OneEntry()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("UnitTest", Entry.UtcFunc);
            DateTime before = DateTime.UtcNow.AddSeconds(-2).ToLocalTime();
            statistic.Add(new Entry());
            DateTime after = DateTime.UtcNow.AddSeconds(2).ToLocalTime();
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.FirstEntry, Is.InRange(before, after));
            Assert.That(statistic.LastEntry, Is.InRange(before, after));
            Assert.That(statistic.Results, Is.Not.Empty);
        }

        [Test]
        public void LocalTime()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("UnitTest", Entry.LocalFunc, true);
            DateTime before = DateTime.Now.AddSeconds(-2);
            statistic.Add(new Entry());
            DateTime after = DateTime.Now.AddSeconds(2);
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.FirstEntry, Is.InRange(before, after));
            Assert.That(statistic.LastEntry, Is.InRange(before, after));
            Assert.That(statistic.Results, Is.Not.Empty);
        }

        [Test]
        public void SetLocalTime()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("UnitTest", Entry.LocalFunc, true);
            statistic.Add(new Entry { Local = DateTime.Today});
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.FirstEntry, Is.EqualTo(DateTime.Today));
            Assert.That(statistic.LastEntry, Is.EqualTo(DateTime.Today));
            Assert.That(statistic.Results, Is.Not.Empty);
        }


        [Test]
        public void SetDateTime()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("UnitTest", Entry.UtcFunc);
            statistic.Add(new Entry {Utc = DateTime.Today.ToUniversalTime()});
            Assert.That(statistic.Count, Is.EqualTo(1));
            Assert.That(statistic.FirstEntry, Is.EqualTo(DateTime.Today));
            Assert.That(statistic.LastEntry, Is.EqualTo(DateTime.Today));
            Assert.That(statistic.Results, Is.Not.Empty);
        }

        [Test]
        public void AddTwoEntries()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("UnitTest", Entry.UtcFunc);
            statistic.Add(new Entry {Utc = DateTime.Today.ToUniversalTime()});
            statistic.Add(new Entry {Utc = DateTime.Today.AddDays(-1).ToUniversalTime()});
            Assert.That(statistic.Count, Is.EqualTo(2));
            Assert.That(statistic.FirstEntry, Is.EqualTo(DateTime.Today.AddDays(-1)));
            Assert.That(statistic.LastEntry, Is.EqualTo(DateTime.Today));
            Assert.That(statistic.Results, Is.Not.Empty);
        }

        [Test]
        public void CompareEmpty()
        {
            TimeBasedStatistic<Entry, string> statistic1 = new TimeBasedStatistic<Entry, string>("Unit Test", Entry.UtcFunc);
            TimeBasedStatistic<Entry, string> statistic2 = new TimeBasedStatistic<Entry, string>("Unit Test", Entry.UtcFunc);
            TimeBasedStatistic<Entry, string> statistic3 = new TimeBasedStatistic<Entry, string>("Earlier", Entry.UtcFunc);

            List<TimeBasedStatistic<Entry, string>> list = new List<TimeBasedStatistic<Entry, string>>
                {
                    statistic1,
                    statistic2,
                    statistic3
                };

            list.Sort();

            Assert.That(statistic1.CompareTo(statistic2), Is.EqualTo(0));
            Assert.That(list[0], Is.EqualTo(statistic3));

            statistic1.Add(new Entry {Utc = DateTime.Today.ToUniversalTime()});
            list.Sort();
            Assert.That(list[0], Is.EqualTo(statistic1)); // one entry
            Assert.That(list[1], Is.EqualTo(statistic3)); // no entries (Earlier)
            Assert.That(list[2], Is.EqualTo(statistic2)); // no entries (Unit Tests)
        }


        [Test]
        public void CompareOne()
        {
            TimeBasedStatistic<Entry, string> statistic1 = new TimeBasedStatistic<Entry, string>("Unit Test", Entry.UtcFunc);
            TimeBasedStatistic<Entry, string> statistic2 = new TimeBasedStatistic<Entry, string>("Unit Test", Entry.UtcFunc);
            TimeBasedStatistic<Entry, string> statistic3 = new TimeBasedStatistic<Entry, string>("Earlier", Entry.UtcFunc);

            List<TimeBasedStatistic<Entry, string>> list = new List<TimeBasedStatistic<Entry, string>>
                {
                    statistic1,
                    statistic2,
                    statistic3
                };

            Entry first = new Entry {Utc = DateTime.Today.ToUniversalTime()};
            Entry last = new Entry {Utc = first.Utc.AddHours(1)};

            statistic1.Add(first);
            statistic2.Add(first);
            statistic2.Add(last);
            statistic3.Add(last);
            list.Sort();
            Assert.That(list[0], Is.EqualTo(statistic2)); // 2 entries
            Assert.That(list[1], Is.EqualTo(statistic1)); // first entry
            Assert.That(list[2], Is.EqualTo(statistic3)); // last entry
        }

        [Test]
        public void CompareOneLocal()
        {
            TimeBasedStatistic<Entry, string> statistic1 = new TimeBasedStatistic<Entry, string>("Unit Test", Entry.LocalFunc, isLocalTime: true);
            TimeBasedStatistic<Entry, string> statistic2 = new TimeBasedStatistic<Entry, string>("Unit Test", Entry.LocalFunc, isLocalTime:true);
            TimeBasedStatistic<Entry, string> statistic3 = new TimeBasedStatistic<Entry, string>("Earlier", Entry.LocalFunc, isLocalTime: true);

            List<TimeBasedStatistic<Entry, string>> list = new List<TimeBasedStatistic<Entry, string>>
                {
                    statistic1,
                    statistic2,
                    statistic3
                };

            Entry first = new Entry { Local = DateTime.Today };
            Entry last = new Entry { Local = first.Local.AddHours(1) };

            statistic1.Add(first);
            statistic2.Add(first);
            statistic2.Add(last);
            statistic3.Add(last);
            list.Sort();
            Assert.That(list[0], Is.EqualTo(statistic2)); // 2 entries
            Assert.That(list[1], Is.EqualTo(statistic1)); // first entry
            Assert.That(list[2], Is.EqualTo(statistic3)); // last entry
        }

        [Test]
        public void TestToString()
        {
            TimeBasedStatistic<Entry, string> statistic = new TimeBasedStatistic<Entry, string>("Time-Unit Test", Entry.UtcFunc);
            string toString = statistic.ToString();
            Assert.That(toString, Is.StringContaining("Time-Unit Test")); // name
            Assert.That(toString, Is.StringContaining("Entry")); // Entry type
            Assert.That(toString, Is.StringContaining("Count: 0")); // count
        }
    }
}