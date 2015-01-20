using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        [Test]
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

        [Test]
        public void AddDataTable()
        {
            DataTable dataTable = new DataTable("Test");
            dataTable.Columns.Add("A", typeof (string));
            dataTable.Columns.Add("B", typeof (int));
            dataTable.Columns.Add("C", typeof (double));

            dataTable.Rows.Add("One", 1, 1.1D);
            dataTable.Rows.Add("Two", 2, 1.2D);
            dataTable.Rows.Add("Three", 3, 1.3D);

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                IWorksheetWriter worksheet = spreadsheet.WriteDataToWorksheet(dataTable, "Test");
                Assert.That(worksheet, Is.Not.Null);
            }

            List<string> result1;
            List<string> result2;
            List<string> result3;
            List<string> result4;

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader worksheet = spreadsheet.ReadWorksheet("Test");
                Assert.That(worksheet, Is.Not.Null);
                result1 = worksheet.ReadRow();
                result2 = worksheet.ReadRow();
                result3 = worksheet.ReadRow();
                result4 = worksheet.ReadRow();
                
            }

            Assert.That(result1, Is.EquivalentTo(new []{"A", "B", "C"}));
            Assert.That(result2, Is.EquivalentTo(new[] { "One", "1", "1.1" }));
            Assert.That(result3, Is.EquivalentTo(new[] { "Two", "2", "1.2" }));
            Assert.That(result4, Is.EquivalentTo(new[] { "Three", "3", "1.3" }));
        }

        [Test]
        public void EmptySpreadsheetIsNotWrittenToDisk()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                Assert.That(spreadsheet.IsReadOnly, Is.False);
            }

            Assert.That(File.Exists(Filename), Is.False, "{0} exists", Filename);
        }


    }

}