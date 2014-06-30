using System.IO;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Packs
{
    [TestFixture]
    public class RemotingExcelReportPackUnitTests : ExcelTestFixture
    {
         [Test]
         public void WithRemotingFiles()
         {
             AmplaProject project = AmplaTestProjects.GetAmplaProject();

             RemotingExcelReportPack reportPack = new RemotingExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             AssertWorksheetExists("Summary");
             AssertWorksheetExists("Sessions");
             AssertWorksheetExists("Remoting");
         }

         [Test]
         public void WithNoRemotingFiles()
         {
             AmplaProject project = AmplaTestProjects.GetWcfOnlyProject();

             RemotingExcelReportPack reportPack = new RemotingExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             AssertWorksheetExists("Summary");
             AssertWorksheetExists("Sessions");
             AssertWorksheetExists("Remoting");
         }
    }
}