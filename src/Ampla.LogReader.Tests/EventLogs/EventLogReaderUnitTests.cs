using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class EventLogReaderUnitTests : TestFixture
    {
        [Test]
        public void ReadEntries()
        {
            ILocalEventLogSystem eventLogSystem = new LocalEventLogSystem();
            EventLog eventLog = eventLogSystem.GetEventLogs().FirstOrDefault(ev => ev.Entries.Count > 0);
            Assert.That(eventLog, Is.Not.Null, "Unable to find an Event Log with something in it.");
            
            EventLogReader reader = new EventLogReader(eventLog);
            Assert.That(reader.Entries, Is.Empty);

            int count = eventLog != null ? eventLog.Entries.Count : -1;
            Assert.That(count, Is.GreaterThan(0), "No entries in the EventLog");

            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
            Assert.That(reader.Entries.Count, Is.EqualTo(count).Or.EqualTo(count + 2));

            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);
            Assert.That(reader.Entries.Count, Is.EqualTo(count).Or.EqualTo(count + 2));
        }
        
    }
}