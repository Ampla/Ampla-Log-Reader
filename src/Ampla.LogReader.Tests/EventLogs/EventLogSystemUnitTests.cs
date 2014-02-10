using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class EventLogSystemUnitTests : TestFixture
    {
        [Test]
        public void GetEventLogs()
        {
            EventLogSystem eventLogSystem = new EventLogSystem();
            IList<EventLog> logs = eventLogSystem.GetEventLogs();
            List<string> nameOfLogs = null;
            // this will throw is there is a security exception accessing the name
            Assert.DoesNotThrow(() => nameOfLogs = logs.Select(eventLog => eventLog.LogDisplayName).ToList());
            Assert.That(nameOfLogs, Is.Not.Empty);
        }

        [Test]
        public void ApplicationEventLog()
        {
            EventLogSystem eventLogSystem = new EventLogSystem();
            EventLog application = eventLogSystem.GetEventLog("Application");
            Assert.That(application, Is.Not.Null);
            Assert.That(application.Entries, Is.Not.Empty);
        }

        [Test]
        public void SystemEventLog()
        {
            EventLogSystem eventLogSystem = new EventLogSystem();
            EventLog system = eventLogSystem.GetEventLog("System");
            Assert.That(system, Is.Not.Null);
            Assert.That(system.Entries, Is.Not.Empty);
        }

        [Test]
        public void SecurityEventLog()
        {
            EventLogSystem eventLogSystem = new EventLogSystem();
            EventLog system = eventLogSystem.GetEventLog("Security");
            Assert.That(system, Is.Null);
        }
    }
}