using System.IO;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Packs
{
    [TestFixture]
    public class WcfExcelReportPackUnitTests : ExcelTestFixture
    {
         [Test]
         public void WithWcfCalls()
         {
             AmplaProject project = AmplaTestProjects.GetAmplaProject();

             WcfExcelReportPack reportPack = new WcfExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             AssertWorksheetExists("Summary");
             AssertWorksheetExists("WcfCalls");
         }
    }
}