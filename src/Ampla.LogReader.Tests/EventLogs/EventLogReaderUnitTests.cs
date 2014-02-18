using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class EventLogReaderUnitTests : TestFixture
    {
        [Test]
        public void ReadEntries()
        {
            EventLogSystem eventLogSystem = new EventLogSystem();
            EventLog eventLog = eventLogSystem.GetEventLog("Microsoft Office Alerts");
            Assert.That(eventLog, Is.Not.Null);
            
            EventLogReader reader = new EventLogReader(eventLog);
            Assert.That(reader.Entries, Is.Empty);

            int count = eventLog.Entries.Count;
            Assert.That(count, Is.GreaterThan(0), "No entries in the EventLog");

            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
            Assert.That(reader.Entries.Count, Is.EqualTo(count));

            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
            Assert.That(reader.Entries.Count, Is.EqualTo(count));
        }
    }
}