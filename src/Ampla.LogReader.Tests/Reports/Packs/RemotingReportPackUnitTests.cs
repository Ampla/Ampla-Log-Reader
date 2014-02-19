using System.IO;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Packs
{
    [TestFixture]
    public class RemotingReportPackUnitTests : ExcelTestFixture
    {
         [Test]
         public void WithRemotingFiles()
         {
             AmplaProject project = AmplaTestProjects.GetAmplaProject();

             RemotingReportPack reportPack = new RemotingReportPack(Filename, project);
             reportPack.Render();

             Assert.That(File.Exists(Filename), Is.True);
         }
    }
}