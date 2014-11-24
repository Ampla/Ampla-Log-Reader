using System;
using System.IO;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Packs
{
    [TestFixture]
    public class WcfExcelReportPackUnitTests : ExcelTestFixture
    {
         [Test]
         public void SimpleAmplaProject()
         {
             AmplaProject project = AmplaTestProjects.GetAmplaProject();

             WcfExcelReportPack reportPack = new WcfExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             AssertWorksheetExists("Summary");
             AssertWorksheetExists("WcfCalls");
             AssertWorksheetExists("Faults");
             AssertWorksheetExists("Locations");
         }

         [Test]
         public void WcfOnlyProject()
         {
             AmplaProject project = AmplaTestProjects.GetWcfOnlyProject();

             WcfExcelReportPack reportPack = new WcfExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             AssertWorksheetExists("Summary");
             AssertWorksheetExists("WcfCalls");
             AssertWorksheetExists("Faults");
             AssertWorksheetExists("Locations");
         }


         [Test]
         public void WcfFaults()
         {
             AmplaProject project = AmplaTestProjects.GetWcfFaultsProject();

             WcfExcelReportPack reportPack = new WcfExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             AssertWorksheetExists("Summary");
             AssertWorksheetExists("WcfCalls");
             AssertWorksheetExists("Faults");
             AssertWorksheetExists("Locations");
         }

        [Test]
        public void TimeZoneReport()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();

            WcfLogDirectory directory = new WcfLogDirectory(project);

            TimeZoneInfo timeZone = TimeZoneHelper.GetSpecificTimeZone("India Standard Time");

            WcfExcelReportPack reportPack = new WcfExcelReportPack(Filename, directory, timeZone);
            reportPack.Render();

            Assert.That(File.Exists(Filename), Is.True);

            AssertWorksheetExists("Summary");
            AssertWorksheetExists("WcfCalls");
            AssertWorksheetExists("Faults");
            AssertWorksheetExists("Locations");
        }

        [Test]
        public void WcfResourceFiles()
        {
            WcfLogDirectory directory = new WcfLogDirectory(@".\Wcf\Resources");

            WcfExcelReportPack reportPack = new WcfExcelReportPack(Filename, directory, TimeZoneInfo.Local);
            reportPack.Render();

            Assert.That(File.Exists(Filename), Is.True);

            AssertWorksheetExists("Summary");
            AssertWorksheetExists("WcfCalls");
            AssertWorksheetExists("Faults");
            AssertWorksheetExists("Locations");
        }

    }
}