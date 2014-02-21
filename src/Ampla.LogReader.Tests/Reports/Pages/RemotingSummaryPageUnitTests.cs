using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Pages
{
    [TestFixture]
    public class RemotingSummaryPageUnitTests : ExcelTestFixture
    {
        private const string logFileName = @".\Remoting\Resources\SingleEntry.log";

        [Test]
        public void NoRecords()
        {
            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                RemotingSummaryPage page = new RemotingSummaryPage(spreadsheet, new List<RemotingEntry>());
                page.Render();
                pageName = page.PageName;
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, pageName))
                {
                    page.ReadLines(10);
                    page.Row(1).AssertValues<string>(Is.Not.Empty);
                    page.Row(2).AssertValues<string>(Is.Not.Empty);

                    page.FindRow(1, value => value.StartsWith("Count"))
                        .Column(2)
                        .AssertValue<string>(Is.Not.EqualTo("0"));
                }
            }
        }

        [Test]
        public void OneRecord()
        {
            ILogReader<RemotingEntry> logReader = new RemotingLogReader(logFileName);
            logReader.Read();

            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                RemotingSummaryPage page = new RemotingSummaryPage(spreadsheet, logReader.Entries);
                page.Render();
                pageName = page.PageName;
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, pageName))
                {
                    page.ReadLines(10);
                    page.Row(1).AssertValues<string>(Is.Not.Empty);
                    page.Row(2).AssertValues<string>(Is.Not.Empty);

                    page.FindRow(
                        1,
                        value => value.StartsWith("Count"))
                        .Column(2)
                        .AssertValue<int>(Is.EqualTo(1));
                }
            }
        }

        [Test]
        public void MultipleRecords()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();

            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                RemotingDirectory directory = new RemotingDirectory(project);
                directory.Read();
                RemotingSummaryPage page = new RemotingSummaryPage(spreadsheet, directory.Entries);
                page.Render();
                pageName = page.PageName;
            }

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, pageName))
                {
                    page.ReadLines(10);
                    page.Row(1).AssertValues<string>(Is.Not.Empty);
                    page.Row(2).AssertValues<string>(Is.Not.Empty);

                    page.FindRow(
                        1,
                        value => value.StartsWith("Count"))
                        .Column(2)
                        .AssertValue<int>(Is.GreaterThan(0));
                }
            }
        }
    }
}