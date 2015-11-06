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
        [Ignore("This is difficult to test")]
        public void CompareSystemEvents()
        {
            CompareLogReaders("System", "Compare.System.evtx", 10);
        }

        [Test]
        [Ignore("This is difficult to test")]
        public void CompareApplicationEvents()
        {
            CompareLogReaders("Application", "Compare.Application.evtx", 10);
        }
        
        private void CompareLogReaders(string eventLogName, string fileName, int entries)
        {
            string evtxFileName = GetSpecificFile(fileName);

            new EventLogExporter().Export(eventLogName, evtxFileName);

            ILogReader<SimpleEventLogEntry> fileReader = new FileEventLogReader(evtxFileName, entries);
            Assert.That(fileReader.Entries, Is.Empty);

            fileReader.ReadAll();
            Assert.That(fileReader.Entries, Is.Not.Empty);

            ILocalEventLogSystem eventLogSystem = new LocalEventLogSystem();
            EventLog eventLog = eventLogSystem.GetEventLog(eventLogName);
            Assert.That(eventLog, Is.Not.Null);

            EventLogReader localReader = new EventLogReader(eventLog, entries);
            Assert.That(localReader.Entries, Is.Empty);

            localReader.ReadAll();

            Assert.That(localReader.Entries, Is.Not.Empty);
            for (int i = 0; i < entries; i++)
            {
                SimpleEventLogEntry fileEntry = fileReader.Entries[i];
                SimpleEventLogEntry localEntry = localReader.Entries[i];

                AssertSame(fileEntry, localEntry, i);
            }
        }

        private void AssertSame(SimpleEventLogEntry actual, SimpleEventLogEntry expected, int index)
        {
            Assert.That(actual.ToString(), Is.EqualTo(expected.ToString()), "Index: {0}\r\n{1}", index, actual);
            Assert.That(actual.CallTime, Is.EqualTo(expected.CallTime), "Calltime");
            Assert.That(actual.Source, Is.EqualTo(expected.Source), "Source");
            Assert.That(actual.Message, Is.EqualTo(expected.Message), "Message");
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName), "UserName");
            Assert.That(actual.MachineName, Is.EqualTo(expected.MachineName), "MachineName");
            Assert.That(actual.EntryType, Is.EqualTo(expected.EntryType), "EntryType");
            Assert.That(actual.Category, Is.EqualTo(expected.Category), "Category");
            Assert.That(actual.Index, Is.EqualTo(expected.Index), "Index");
            Assert.That(actual.InstanceId, Is.EqualTo(expected.InstanceId), "InstanceId");
        }
    }
}