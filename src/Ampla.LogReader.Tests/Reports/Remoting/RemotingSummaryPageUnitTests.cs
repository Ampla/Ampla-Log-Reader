using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Remoting
{
    [TestFixture]
    public class RemotingSummaryPageUnitTests : ExcelTestFixture
    {
        private const string logFileName = @".\Remoting\Resources\SingleEntry.log";

        [Test]
        public void NoRecords()
        {
            RemotingSummaryPage page;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                page = new RemotingSummaryPage(spreadsheet, new List<RemotingEntry>());
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
            ILogReader<RemotingEntry> logReader = new RemotingLogReader(logFileName);
            logReader.Read();

            RemotingSummaryPage page;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                page = new RemotingSummaryPage(spreadsheet, logReader.Entries);
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

            RemotingSummaryPage page;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                RemotingDirectory directory = new RemotingDirectory(project);
                directory.Read();
                page = new RemotingSummaryPage(spreadsheet, directory.Entries);
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