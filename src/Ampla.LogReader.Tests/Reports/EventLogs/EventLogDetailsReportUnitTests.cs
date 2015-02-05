using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.ReportWriters;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.EventLogs
{
    [TestFixture]
    public class EventLogDetailsReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             EventLog eventLog = new LocalEventLogSystem().GetEventLog("Application");
             SimpleReportWriter writer = new SimpleReportWriter();
             EventLogDetailsReport report = new EventLogDetailsReport(eventLog, new List<SimpleEventLogEntry>(), writer);
             
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}