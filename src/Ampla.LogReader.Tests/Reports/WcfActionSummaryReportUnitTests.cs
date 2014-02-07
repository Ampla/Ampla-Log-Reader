using System;
using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports
{
    [TestFixture]
    public class WcfActionSummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             SimpleReportWriter writer = new SimpleReportWriter();
             WcfActionSummaryReport report = new WcfActionSummaryReport(new List<WcfCall>(), writer);
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}