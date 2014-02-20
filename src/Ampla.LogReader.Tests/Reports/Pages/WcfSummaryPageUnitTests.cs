using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.Reports.Pages;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Wcf
{
    [TestFixture]
    public class WcfSummaryPageUnitTests : ExcelTestFixture
    {
        private const string logFileName = @".\Wcf\Resources\SingleEntry.log";

        [Test]
        public void NoRecords()
        {
            WcfSummaryPage page;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                page = new WcfSummaryPage(spreadsheet, new List<WcfCall>());
                page.Render();
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (IWorksheetReader reader = spreadsheet.ReadWorksheet(page.PageName))
                {
                    List<string> row1 = reader.ReadRow();
                    Assert.That(row1, Is.Not.Empty);
                }
            }
        }

        [Test]
        public void OneRecord()
        {
            ILogReader<WcfCall> logReader = new WcfLogReader(logFileName);
            logReader.Read();

            WcfSummaryPage page;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                page = new WcfSummaryPage(spreadsheet, logReader.Entries);
                page.Render();
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (IWorksheetReader reader = spreadsheet.ReadWorksheet(page.PageName))
                {
                    List<string> row1 = reader.ReadRow();
                    Assert.That(row1, Is.Not.Empty);
                    List<string> row2 = reader.ReadRow();
                    Assert.That(row2, Is.EquivalentTo(new [] {"Count: ", "1"}));

                    List<string> row3 = reader.ReadRow();
                    Assert.That(row3, Is.Not.Empty);

                    List<string> row4 = reader.ReadRow();
                    Assert.That(row4, Is.Not.Empty);

                    List<string> row5 = reader.ReadRow();
                    Assert.That(row5, Is.Not.Empty);

                }
            }
        }

        [Test]
        public void MultipleRecords()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();

            WcfSummaryPage page;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                WcfLogDirectory directory = new WcfLogDirectory(project);
                directory.Read();
                page = new WcfSummaryPage(spreadsheet, directory.Entries);
                page.Render();
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (IWorksheetReader reader = spreadsheet.ReadWorksheet(page.PageName))
                {
                    List<string> row1 = reader.ReadRow();
                    Assert.That(row1, Is.Not.Empty);
                    List<string> row2 = reader.ReadRow();
                    Assert.That(row2, Is.Not.Empty);

                    int count = int.Parse(row2[1]);
                    Assert.That(count, Is.GreaterThan(1));

                    List<string> row3 = reader.ReadRow();
                    Assert.That(row3, Is.Not.Empty);

                    List<string> row4 = reader.ReadRow();
                    Assert.That(row4, Is.Not.Empty);

                    List<string> row5 = reader.ReadRow();
                    Assert.That(row5, Is.Not.Empty);

                }
            }
        }
    }
}