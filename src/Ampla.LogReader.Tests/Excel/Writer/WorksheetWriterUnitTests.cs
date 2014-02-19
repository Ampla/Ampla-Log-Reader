using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel.Reader;
using NUnit.Framework;

namespace Ampla.LogReader.Excel.Writer
{
    [TestFixture]
    public class WorksheetWriterUnitTests : ExcelTestFixture
    {
        [Test]
        public void NullConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new WorksheetWriter(null));
        }

        [Test]
        public void WriteRow()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    List<string> list = new List<string> { "One", "Two", "Three" };

                    writer.WriteRow(list);
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A2"));
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader reader = spreadsheet.ReadWorksheet("UnitTests");
                List<string> list = reader.ReadRow();
                Assert.That(list, Is.Not.Empty);
                Assert.That(list, Is.EquivalentTo(new List<string> { "One", "Two", "Three" }));
            }
        }

        [Test]
        public void Write2Rows()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    writer.WriteRow(new List<string> { "One", "Two", "Three" });
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A2"));
                    writer.WriteRow(new List<string> { "Four", "Five", "Six" });
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A3"));
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader reader = spreadsheet.ReadWorksheet("UnitTests");
                List<string> list = reader.ReadRow();
                Assert.That(list, Is.Not.Empty);
                Assert.That(list, Is.EquivalentTo(new List<string> { "One", "Two", "Three" }));
                list = reader.ReadRow();
                Assert.That(list, Is.Not.Empty);
                Assert.That(list, Is.EquivalentTo(new List<string> { "Four", "Five", "Six" }));
            }
        }

        /// <summary>
        ///  Row 1 is blank
        ///  Row 2 = blank, "One", "Two", "Three"
        ///  Row 3 = blank, "Four", "Five", "Six"
        /// </summary>
        [Test]
        public void MoveToAndWrite()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    writer.MoveTo("B2");
                    writer.WriteRow(new List<string> { "One", "Two", "Three" });
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("B3"));
                    writer.WriteRow(new List<string> { "Four", "Five", "Six" });
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("B4"));
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (IWorksheetReader reader = spreadsheet.ReadWorksheet("UnitTests"))
                {
                    Assert.That(reader.GetCurrentCell().Address, Is.EqualTo("A1"));
                    Assert.That(reader.ReadRow(), Is.EquivalentTo(new List<string>()));

                    reader.MoveTo("B2");
                    Assert.That(reader.GetCurrentCell().Address, Is.EqualTo("B2"));
                    Assert.That(reader.ReadRow(), Is.EquivalentTo(new List<string> { "One", "Two", "Three" }));
                    Assert.That(reader.GetCurrentCell().Address, Is.EqualTo("B3"));
                    Assert.That(reader.ReadRow(), Is.EquivalentTo(new List<string> { "Four", "Five", "Six" }));
                }
            }
        }

        [Test]
        public void GetCurrentCell()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    writer.MoveTo("B2");
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("B2"));

                    writer.MoveTo(10, 20);
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("T10"));
                }
            }
        }

        [Test]
        public void Write()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    writer.Write("One");
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("B1"));

                    writer.MoveTo("D3");
                    writer.Write("New value at D3");
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("E3"));
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader reader = spreadsheet.ReadWorksheet("UnitTests");
                reader.MoveTo("A1");
                Assert.That(reader.Read(), Is.EqualTo("One"));
                Assert.That(reader.GetCurrentCell().Address, Is.EqualTo("B1"));
                reader.MoveTo("D3");
                Assert.That(reader.Read(), Is.EqualTo("New value at D3"));
                Assert.That(reader.GetCurrentCell().Address, Is.EqualTo("E3"));
            }
        }

        [Test]
        public void WriteRows()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    writer.WriteRow(); // A1
                    writer.WriteRow("One"); // A2
                    writer.WriteRow("One", "Two"); // A3:B3
                    writer.WriteRow("One", "Two", "Three"); // A4:C4
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A5"));
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader reader = spreadsheet.ReadWorksheet("UnitTests");
                List<string> row1 = reader.ReadRow();
                List<string> row2 = reader.ReadRow();
                List<string> row3 = reader.ReadRow();
                List<string> row4 = reader.ReadRow();
                Assert.That(row1, Is.Empty);
                Assert.That(row2, Is.Not.Empty);
                Assert.That(row2, Is.EquivalentTo(new[] { "One" }));
                Assert.That(row3, Is.Not.Empty);
                Assert.That(row3, Is.EquivalentTo(new[] { "One", "Two" }));
                Assert.That(row4, Is.Not.Empty);
                Assert.That(row4, Is.EquivalentTo(new[] { "One", "Two", "Three" }));
            }
        }

        [Test]
        public void WriteRowsNullParams()
        {
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                using (IWorksheetWriter writer = spreadsheet.WriteToWorksheet("UnitTests"))
                {
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A1"));
                    writer.WriteRow((string[]) null); // A1
                    Assert.That(writer.GetCurrentCell().Address, Is.EqualTo("A2"));
                }
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader reader = spreadsheet.ReadWorksheet("UnitTests");
                List<string> row1 = reader.ReadRow();
                Assert.That(row1, Is.Empty);
            }
        }

    }
}