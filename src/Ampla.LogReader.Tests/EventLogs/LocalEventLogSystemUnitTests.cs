using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class LocalEventLogSystemUnitTests : TestFixture
    {
        private LocalEventLogSystem eventLogSystem;

        protected override void OnSetUp()
        {
            base.OnSetUp();
            eventLogSystem = new LocalEventLogSystem();
        }

        [Test]
        public void GetEventLogs()
        {
            IEnumerable<EventLog> logs = eventLogSystem.GetEventLogs();
            List<string> nameOfLogs = null;
            // this will throw is there is a security exception accessing the name property
            Assert.DoesNotThrow(() => nameOfLogs = logs.Select(eventLog => eventLog.LogDisplayName).ToList());
            Assert.That(nameOfLogs, Is.Not.Empty);
        }

        [Test]
        public void ApplicationEventLog()
        {
            EventLog application = eventLogSystem.GetEventLog("Application");
            Assert.That(application, Is.Not.Null);
            Assert.That(application.Entries, Is.Not.Empty);
        }

        [Test]
        public void SystemEventLog()
        {
            EventLog system = eventLogSystem.GetEventLog("System");
            Assert.That(system, Is.Not.Null);
            Assert.That(system.Entries, Is.Not.Empty);
        }

        [Test]
        public void SecurityEventLog()
        {
            // this can throw if the Security Event Log does not have permission to view.
            Assert.DoesNotThrow(() => eventLogSystem.GetEventLog("Security"));
        }

        [Test]
        public void GetReaders()
        {
            IEnumerable<ILogReader<SimpleEventLogEntry>> readers = eventLogSystem.GetReaders();
            Assert.That(readers, Is.Not.Empty);
            foreach (EventLogReader reader in readers)
            {
                Assert.That(reader.Name, Is.Not.Empty);
            }
        }
    }
}