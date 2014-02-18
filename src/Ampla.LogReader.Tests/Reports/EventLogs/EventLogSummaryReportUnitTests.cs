using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.ReportWriters;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.EventLogs
{
    [TestFixture]
    public class EventLogSummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             EventLog eventLog = new EventLogSystem().GetEventLog("Application");
             SimpleReportWriter writer = new SimpleReportWriter();
             EventLogSummaryReport report = new EventLogSummaryReport(eventLog, new List<SimpleEventLogEntry>(), writer);
             
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}