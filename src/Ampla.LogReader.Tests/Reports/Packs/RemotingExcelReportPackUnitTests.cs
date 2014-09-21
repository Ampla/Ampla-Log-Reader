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

             using (ExcelPage page = AssertWorksheetContainsData("Summary", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Not.Empty);
             }
             
             using (ExcelPage page = AssertWorksheetContainsData("Sessions", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Not.Empty);
             }
             
             using (ExcelPage page = AssertWorksheetContainsData("Remoting", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Not.Empty);
             }

             using (ExcelPage page = AssertWorksheetContainsData("Locations", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Not.Empty);
             }
         }

         [Test]
         public void WithNoRemotingFiles()
         {
             AmplaProject project = AmplaTestProjects.GetWcfOnlyProject();

             RemotingExcelReportPack reportPack = new RemotingExcelReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);

             using (ExcelPage page = AssertWorksheetContainsData("Summary", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Not.Empty);
             }
            
             using (ExcelPage page = AssertWorksheetContainsData("Sessions", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.EqualTo(new string[] {"None"}));
             }
             
             using (ExcelPage page = AssertWorksheetContainsData("Remoting", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Empty);
             }

             using (ExcelPage page = AssertWorksheetContainsData("Locations", 10))
             {
                 page.Row(1).AssertValues<string>(Is.Not.Empty);
                 page.Row(2).AssertValues<string>(Is.Empty);
             }
         }
    }
}