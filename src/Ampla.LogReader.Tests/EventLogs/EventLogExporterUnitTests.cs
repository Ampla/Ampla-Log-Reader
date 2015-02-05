using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs
{
    [TestFixture]
    public class EventLogExporterUnitTests : TestFixture
    {
        protected override void OnFixtureSetUp()
        {
            base.OnFixtureSetUp();
            string directory = @"Files\" + GetType().Name;
            TempDirectory = new TempDirectory(directory, "UnitTest_{0:00}.evtx");
            TempDirectory.DeleteAllFiles();
        }

        private TempDirectory TempDirectory { get; set; }

        [Test]
        public void Application()
        {
            string fileName = TempDirectory.GetSpecificFileName("Application.evtx");

            IEventLogExporter exporter = new EventLogExporter();
            exporter.Export("Application", fileName);
            Assert.That(File.Exists(fileName), Is.True);
        }

        [Test]
        public void System()
        {
            string fileName = TempDirectory.GetSpecificFileName("System.evtx");

            IEventLogExporter exporter = new EventLogExporter();
            Assert.That(File.Exists(fileName), Is.False);
            exporter.Export("System", fileName);

            Assert.That(File.Exists(fileName), Is.True);
        }

        [Test]
        public void HardwareEvents()
        {
            string fileName = TempDirectory.GetSpecificFileName("HardwareEvents.evtx");

            IEventLogExporter exporter = new EventLogExporter();
            Assert.That(File.Exists(fileName), Is.False);
            exporter.Export("HardwareEvents", fileName);

            Assert.That(File.Exists(fileName), Is.True);
        }
    }
}