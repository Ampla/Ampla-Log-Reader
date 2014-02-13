using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Wcf
{
    [TestFixture]
    public class WcfFaultSummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             SimpleReportWriter writer = new SimpleReportWriter();
             WcfFaultSummaryReport report = new WcfFaultSummaryReport(new List<WcfCall>(), writer);
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}