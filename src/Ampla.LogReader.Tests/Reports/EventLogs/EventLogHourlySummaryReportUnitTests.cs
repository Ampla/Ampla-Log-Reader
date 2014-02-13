using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.ReportWriters;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.EventLogs
{
    [TestFixture]
    public class EventLogHourlySummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             SimpleReportWriter writer = new SimpleReportWriter();
             EventLogHourlySummaryReport report = new EventLogHourlySummaryReport("Application", new List<EventLogEntry>(), writer);
             
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}