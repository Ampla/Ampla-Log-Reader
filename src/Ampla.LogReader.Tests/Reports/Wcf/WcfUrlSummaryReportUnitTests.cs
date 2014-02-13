using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Wcf
{
    [TestFixture]
    public class WcfUrlSummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             SimpleReportWriter writer = new SimpleReportWriter();
             WcfUrlSummaryReport report = new WcfUrlSummaryReport(new List<WcfCall>(), writer);
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}