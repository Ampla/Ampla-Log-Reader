﻿using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports
{
    [TestFixture]
    public class WcfHourlySummaryReportUnitTests : TestFixture
    {
         [Test]
         public void EmptyCalls()
         {
             SimpleReportWriter writer = new SimpleReportWriter();
             WcfHourlySummaryReport report = new WcfHourlySummaryReport(new List<WcfCall>(), writer);
             report.Render();

             Assert.That(writer.ToString(), Is.Not.Empty);
         }
     
    }
}