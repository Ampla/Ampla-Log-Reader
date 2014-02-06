using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Excel.Writer;
using NUnit.Framework;

namespace Ampla.LogReader.Excel
{
    [TestFixture]
    public class ExcelSpreadsheetUnitTests : ExcelTestFixture
    {
        [Test]
        public void NullFilename()
        {
            Assert.Throws<ArgumentNullException>(() => ExcelSpreadsheet.CreateNew(null));
        }

        [Test]
        public void EmptyFilename()
        {
            Assert.Throws<ArgumentException>(() => ExcelSpreadsheet.CreateNew(""));
        }

        public void ReadOnlyWorksheet()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                Assert.That(spreadsheet.IsReadOnly, Is.False);
                IWorksheetWriter worksheet = spreadsheet.WriteToWorksheet("New sheet");
                worksheet.WriteRow(new List<string> { "One", "Two", "Three" });
                Assert.That(worksheet, Is.Not.Null);
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                Assert.That(spreadsheet.IsReadOnly, Is.True);
                Assert.Throws<InvalidOperationException>(() => spreadsheet.WriteToWorksheet("Should fail"));
            }
        }

        [Test]
        public void ReadAndWrite()
        {
            List<string> list = new List<string> { "One", "Two", "Three" };
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                IWorksheetWriter worksheet = spreadsheet.WriteToWorksheet("New sheet");
                Assert.That(worksheet, Is.Not.Null);
                worksheet.WriteRow(list);
            }

            List<string> result;

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader worksheet = spreadsheet.ReadWorksheet("New sheet");
                Assert.That(worksheet, Is.Not.Null);
                result = worksheet.ReadRow();
            }

            Assert.That(result, Is.EquivalentTo(list));
        }


    }

}