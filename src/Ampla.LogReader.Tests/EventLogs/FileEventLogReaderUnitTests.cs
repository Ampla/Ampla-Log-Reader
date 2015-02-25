using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class FileEventLogReaderUnitTests : TemporaryFilesTestFixture
    {
        public FileEventLogReaderUnitTests() : base("evtx")
        {
        }

        [Test]
        public void ReadApplication()
        {
            string fileName = GetSpecificFile("Application.All.evtx");

            new EventLogExporter().Export("Application", fileName);

            ILogReader<SimpleEventLogEntry> reader = new FileEventLogReader(fileName);
            Assert.That(reader.Entries, Is.Empty);

            reader.ReadAll();
            Assert.That(reader.Entries, Is.Not.Empty);

            SimpleEventLogEntry firstEntry = reader.Entries[0];
            Assert.That(firstEntry.Message, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void Read10Entries()
        {
            string fileName = GetSpecificFile("Application.10.evtx");

            new EventLogExporter().Export("Application", fileName);

            ILogReader<SimpleEventLogEntry> reader = new FileEventLogReader(fileName, 10);
            Assert.That(reader.Entries, Is.Empty);

            reader.ReadAll();
            Assert.That(reader.Entries.Count, Is.EqualTo(10));

            SimpleEventLogEntry firstEntry = reader.Entries[0];
            Assert.That(firstEntry.Message, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void ReadEmpty()
        {
            string fileName = GetSpecificFile("HardwareEvents.evtx");
            new EventLogExporter().Export("HardwareEvents", fileName);

            ILogReader<SimpleEventLogEntry> reader = new FileEventLogReader(fileName);
            Assert.That(reader.Entries, Is.Empty);

            reader.ReadAll();
            Assert.That(reader.Entries, Is.Empty);
        }
    }
}