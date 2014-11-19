using System;
using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Data
{
    public class WcfCallTableUnitTests : ExcelTestFixture
    {
        private const string logFileName = @".\Wcf\Resources\SingleEntry.log";

        [Test]
        public void CreateDataTable()
        {
            WcfLogReader reader = new WcfLogReader(logFileName);
            reader.Read();
            
            Assert.That(reader.Entries, Is.Not.Empty);

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                DataTable table = new WcfCallTable(reader.Entries, null).Data;
                spreadsheet.WriteDataToWorksheet(table, "WcfCalls");
            }

            List<string> header;
            List<string> result;

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                IWorksheetReader worksheet = spreadsheet.ReadWorksheet("WcfCalls");
                Assert.That(worksheet, Is.Not.Null);
                header = worksheet.ReadRow();
                result = worksheet.ReadRow();
            }

            Assert.That(header, Is.Not.Empty);
            Assert.That(result, Is.Not.Empty);

            Assert.That(header[0], Is.EqualTo("Id"));
            Assert.That(result[0], Is.EqualTo("1"));
        }


        [Test]
        public void LocalTimeColumnUsesTimeZone()
        {
            WcfLogReader reader = new WcfLogReader(logFileName);
            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);

            DateTime callTimeUtc = reader.Entries[0].CallTime;

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                DataTable localTable = new WcfCallTable(reader.Entries, TimeZoneInfo.Local).Data;
                spreadsheet.WriteDataToWorksheet(localTable, "LocalTime");

                DataTable utcTable = new WcfCallTable(reader.Entries, TimeZoneInfo.Utc).Data;
                spreadsheet.WriteDataToWorksheet(utcTable, "UtcTime");

                Assert.That(localTable.Columns["CallTimeLocal"], Is.Not.Null);
                Assert.That(localTable.Columns["CallTimeUtc"], Is.Not.Null);

                Assert.That(localTable.Rows.Count, Is.EqualTo(1));
                Assert.That(utcTable.Rows.Count, Is.EqualTo(1));

                Assert.That(localTable.Rows[0]["CallTimeLocal"], Is.EqualTo(callTimeUtc.ToLocalTime()));
                Assert.That(utcTable.Rows[0]["CallTimeLocal"], Is.EqualTo(callTimeUtc.ToUniversalTime()));
            }
        }

        [Test]
        public void IndiaStandardTime()
        {
            WcfLogReader reader = new WcfLogReader(logFileName);
            reader.Read();

            Assert.That(reader.Entries, Is.Not.Empty);

            DateTime callTimeUtc = reader.Entries[0].CallTime;

            TimeZoneInfo ist = TimeZoneHelper.GetSpecificTimeZone("India Standard Time");
            Assert.That(ist, Is.Not.Null);
            
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                DataTable localTable = new WcfCallTable(reader.Entries,TimeZoneInfo.Local).Data;
                spreadsheet.WriteDataToWorksheet(localTable, "LocalTime");

                DataTable istTable = new WcfCallTable(reader.Entries,ist).Data;
                spreadsheet.WriteDataToWorksheet(istTable, "IST Time");

                Assert.That(localTable.Columns["CallTimeLocal"], Is.Not.Null);

                Assert.That(localTable.Rows.Count, Is.EqualTo(1));
                Assert.That(istTable.Rows.Count, Is.EqualTo(1));

                Assert.That(localTable.Rows[0]["CallTimeLocal"], Is.EqualTo(callTimeUtc.ToLocalTime()));
                Assert.That(istTable.Rows[0]["CallTimeLocal"], Is.EqualTo(TimeZoneInfo.ConvertTimeFromUtc(callTimeUtc, ist)));
            }
        }
    }
}