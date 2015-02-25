using System;
using System.IO;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.Excel;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Packs
{
    [TestFixture]
    public class EventLogExcelReportPackUnitTests : ExcelTestFixture
    {
        [Test]
        public void LocalEventLogReport()
        {
            IEventLogSystem eventLogSystem = new LocalEventLogSystem();
            EventLogReportPack reportPack = new EventLogReportPack(Filename, eventLogSystem);
            reportPack.Render();

            Assert.That(File.Exists(Filename), Is.True);

            AssertWorksheetExists("Summary");
            AssertWorksheetContainsData("Application", 2);
            AssertWorksheetContainsData("System", 2);
        }

        [Test]
        public void FileEventLogReport()
        {
            string application = GetSpecificFile("Application.evtx");
            string hardwareEvents = GetSpecificFile("HardwareEvents.evtx");

            IEventLogExporter exporter = new EventLogExporter();
            exporter.Export("Application", application);
            exporter.Export("HardwareEvents", hardwareEvents);

            IEventLogSystem eventLogSystem = new FileEventLogSystem(application, hardwareEvents);
            EventLogReportPack reportPack = new EventLogReportPack(Filename, eventLogSystem);
            reportPack.Render();

            Assert.That(File.Exists(Filename), Is.True);

            AssertWorksheetExists("Summary");
            AssertWorksheetContainsData("Application", 2);
            AssertWorksheetExists("HardwareEvents.evtx");
        }
    }
}