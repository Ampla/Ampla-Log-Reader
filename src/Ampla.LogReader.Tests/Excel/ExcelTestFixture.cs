using System;
using NUnit.Framework;

namespace Ampla.LogReader.Excel
{
    [TestFixture]
    public abstract class ExcelTestFixture : TemporaryFilesTestFixture
    {
        protected ExcelTestFixture() : base("xlsx")
        {
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            Filename = GetNextTemporaryFile();
        }

        protected string Filename { get; private set; }

        protected void AssertWorksheetExists(string worksheet)
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                string[] available = spreadsheet.GetWorksheetNames();
                Assert.That(available, Contains.Item(worksheet), "{0} worksheet does not exist in {1}", worksheet, Filename);
            }
        }

        protected ExcelPage AssertWorksheetContainsData(string worksheet, int rows)
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheet ws = spreadsheet.GetWorksheet(worksheet);
                Assert.That(ws, Is.Not.Null, "{0} worksheet does not exist in {1}", worksheet, Filename);

                ExcelPage page = new ExcelPage(spreadsheet, worksheet);
                page.ReadLines(rows);
                return page;
            }
        }

    }
}