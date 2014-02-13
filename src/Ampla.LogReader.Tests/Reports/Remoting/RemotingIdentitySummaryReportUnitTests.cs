using System.Collections.Generic;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.ReportWriters;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Remoting
{
    [TestFixture]
    public class RemotingIdentitySummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             SimpleReportWriter writer = new SimpleReportWriter();
             RemotingIdentitySummaryReport report = new RemotingIdentitySummaryReport(new List<RemotingEntry>(), writer);
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}