using Ampla.LogReader.Excel.Reader;
using NUnit.Framework;
using System.Collections.Generic;

namespace Ampla.LogReader.Excel
{
    [TestFixture]
    public abstract class ExcelTestFixture : TestFixture
    {
        protected override void OnFixtureSetUp()
        {
            base.OnFixtureSetUp();
            string directory = @"Files\" + GetType().Name;
            TempDirectory = new TempDirectory(directory, "UnitTest_{0:00}.xlsx");
            TempDirectory.DeleteAllFiles();
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Filename = TempDirectory.GetNextTemporaryFile();
        }

        protected string Filename { get; private set; }

        private TempDirectory TempDirectory { get; set; }


        protected void AssertWorksheetExists(string worksheet)
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheet ws = spreadsheet.GetWorksheet(worksheet);
                Assert.That(ws, Is.Not.Null, "{0} worksheet does not exist in {1}", worksheet, Filename);
            }
        }

    }
}