using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    public class WcfCallExcelUnitTests : ExcelTestFixture
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
                DataTable table = WcfCall.CreateDataTable(reader.Entries);
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
    }
}