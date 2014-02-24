using System;
using NUnit.Framework;

namespace Ampla.LogReader.Excel
{
    [TestFixture]
    public class ExcelPageUnitTests : ExcelTestFixture
    {
        [Test]
        public void Row()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (var writer = spreadsheet.WriteToWorksheet("Data"))
                {
                    writer.WriteRow("Column 1", "Column 2", "Column 3");
                    writer.WriteRow("A2", "B2", "C2");
                    writer.WriteRow("A3", "B3", "C3");
                    writer.WriteRow("A4", "B4", "C4");
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, "Data"))
                {
                    page.ReadLines(4);
                    page.Row(1).AssertValues<string>(Is.EquivalentTo(new[] { "Column 1", "Column 2", "Column 3" }));
                    page.Row(2).AssertValues<string>(Is.EquivalentTo(new[] { "A2", "B2", "C2" }));
                    page.Row(3).AssertValues<string>(Is.EquivalentTo(new[] { "A3", "B3", "C3" }));
                    page.Row(4).AssertValues<string>(Is.EquivalentTo(new[] { "A4", "B4", "C4" }));

                    Assert.Throws<AssertionException>(() => page.Row(5));
                }
            }
        }

        [Test]
        public void RowWithExtraRows()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (var writer = spreadsheet.WriteToWorksheet("Data"))
                {
                    writer.WriteRow("Column 1", "Column 2", "Column 3");
                    writer.WriteRow("A2", "B2", "C2");
                    writer.WriteRow("A3", "B3", "C3");
                    writer.WriteRow("A4", "B4", "C4");
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, "Data"))
                {
                    page.ReadLines(10);
                    page.Row(1).AssertValues<string>(Is.EquivalentTo(new[] { "Column 1", "Column 2", "Column 3" }));
                    page.Row(2).AssertValues<string>(Is.EquivalentTo(new[] { "A2", "B2", "C2" }));
                    page.Row(3).AssertValues<string>(Is.EquivalentTo(new[] { "A3", "B3", "C3" }));
                    page.Row(4).AssertValues<string>(Is.EquivalentTo(new[] { "A4", "B4", "C4" }));

                    page.Row(5).AssertValues<string>(Is.Empty);
                }
            }
        }

        [Test]
        public void FindRow()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (var writer = spreadsheet.WriteToWorksheet("Data"))
                {
                    writer.WriteRow("Column 1", "Column 2", "Column 3");
                    writer.WriteRow("A2", "B2", "C2");
                    writer.WriteRow("A3", "B3", "C3");
                    writer.WriteRow("A4", "B4", "C4");
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, "Data"))
                {
                    page.ReadLines(4);
                    page.FindRow(1, s => s.EndsWith("3")).AssertValues<string>(Is.EquivalentTo(new[] { "A3", "B3", "C3" }));
                    
                    Assert.Throws<AssertionException>(() => page.FindRow(1, s => s.StartsWith("X")));
                }
            }
        }

        [Test]
        public void FindRowWithExtraRows()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (var writer = spreadsheet.WriteToWorksheet("Data"))
                {
                    writer.WriteRow("Column 1", "Column 2", "Column 3");
                    writer.WriteRow("A2", "B2", "C2");
                    writer.WriteRow("A3", "B3", "C3");
                    writer.WriteRow("A4", "B4", "C4");
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, "Data"))
                {
                    page.ReadLines(10);
                    page.FindRow(1, s => s.EndsWith("3")).AssertValues<string>(Is.EquivalentTo(new[] { "A3", "B3", "C3" }));

                    Assert.Throws<AssertionException>(() => page.FindRow(1, s => s.StartsWith("X")));
                }
            }
        }

    }
}