using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class FileEventLogSystemUnitTests : TemporaryFilesTestFixture
    {
        private const string hardwareEventsLog = @".\EventLogs\Resources\Empty.evtx";

        public FileEventLogSystemUnitTests() : base("xlsx")
        {
        }

        [Test]
        public void EmptyFile()
        {
            Assert.That(File.Exists(hardwareEventsLog), Is.True);
            FileEventLogSystem eventLogSystem = new FileEventLogSystem(hardwareEventsLog);

            IEnumerable<ILogReader<SimpleEventLogEntry>> readers = eventLogSystem.GetReaders();
            Assert.That(readers, Is.Not.Empty);
            foreach (ILogReader<SimpleEventLogEntry> reader in readers)
            {
                Assert.That(reader.Name, Is.EqualTo("Empty.evtx"));
            }
        }

        [Test]
        public void MultipleFiles()
        {
            Assert.That(File.Exists(hardwareEventsLog), Is.True);

            string systemLog = GetSpecificFile("System.evtx");

            new EventLogExporter().Export("System", systemLog);

            FileEventLogSystem eventLogSystem = new FileEventLogSystem(hardwareEventsLog, systemLog);

            List<ILogReader<SimpleEventLogEntry>> readers = eventLogSystem.GetReaders().ToList();
            Assert.That(readers, Is.Not.Empty);
            Assert.That(readers.Count, Is.EqualTo(2));

            Assert.That(readers[0].Name, Is.EqualTo("Empty.evtx"));
            Assert.That(readers[1].Name, Is.EqualTo("System.evtx"));
        }

        [Test]
        public void InvalidFiles()
        {
            Assert.That(File.Exists(hardwareEventsLog), Is.True);

            FileEventLogSystem eventLogSystem = new FileEventLogSystem(hardwareEventsLog, "Invalid.evtx");

            List<ILogReader<SimpleEventLogEntry>> readers = eventLogSystem.GetReaders().ToList();
            Assert.That(readers, Is.Not.Empty);
            Assert.That(readers.Count, Is.EqualTo(1));

            Assert.That(readers[0].Name, Is.EqualTo("Empty.evtx"));
        }


        [Test]
        public void CompareLocalvsFileReaders()
        {

            string fileName = GetSpecificFile("Application.evtx");

            new EventLogExporter().Export("Application", fileName);

            ILogReader<SimpleEventLogEntry> fileReader = new FileEventLogReader(fileName, 10);
            Assert.That(fileReader.Entries, Is.Empty);

            fileReader.ReadAll();
            Assert.That(fileReader.Entries, Is.Not.Empty);


            ILocalEventLogSystem eventLogSystem = new LocalEventLogSystem();
            EventLog eventLog = eventLogSystem.GetEventLog("Application");
            Assert.That(eventLog, Is.Not.Null);

            EventLogReader localReader = new EventLogReader(eventLog, 10);
            Assert.That(localReader.Entries, Is.Empty);

            localReader.ReadAll();

            Assert.That(localReader.Entries, Is.Not.Empty);

            AssertSame(fileReader.Entries[0], localReader.Entries[0]);
        }

        private void AssertSame(SimpleEventLogEntry x, SimpleEventLogEntry y)
        {
            Assert.That(x.ToString(), Is.EqualTo(y.ToString()));
            Assert.That(x.CallTime, Is.EqualTo(y.CallTime), "Calltime");
            Assert.That(x.Source, Is.EqualTo(y.Source), "Source");
            Assert.That(x.Message, Is.EqualTo(y.Message), "Message");
            Assert.That(x.UserName, Is.EqualTo(y.UserName), "UserName");
            Assert.That(x.MachineName, Is.EqualTo(y.MachineName), "MachineName");
            Assert.That(x.EntryType, Is.EqualTo(y.EntryType), "EntryType");
            Assert.That(x.Category, Is.EqualTo(y.Category), "Category");
            Assert.That(x.Index, Is.EqualTo(y.Index), "Index");
            Assert.That(x.InstanceId, Is.EqualTo(y.InstanceId), "InstanceId");
        }
    }
}