using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class FileEventLogSystemUnitTests : TestFixture
    {
        private FileEventLogSystem eventLogSystem;

        private const string hardwareEventsLog = @".\EventLogs\Resources\Empty.evtx";

        [Test]
        public void EmptyFile()
        {
            Assert.That(File.Exists(hardwareEventsLog), Is.True);
            eventLogSystem = new FileEventLogSystem(hardwareEventsLog);

            IEnumerable<ILogReader<SimpleEventLogEntry>> readers = eventLogSystem.GetReaders();
            Assert.That(readers, Is.Not.Empty);
            foreach (ILogReader<SimpleEventLogEntry> reader in readers)
            {
                Assert.That(reader.Name, Is.EqualTo("Empty.evtx"));
            }
        }
    }
}