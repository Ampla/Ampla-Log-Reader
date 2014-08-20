using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Pages
{
    [TestFixture]
    public class RemotingSessionSummaryPageUnitTests : ExcelTestFixture
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
                RemotingSessionSummaryPage page = new RemotingSessionSummaryPage(spreadsheet, logReader.Entries);
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
                    page.Row(3).AssertValues<string>(Is.Not.Empty);

                    page.FindRow(
                        2,
                        value => value == "9f061bbe-a80d-4881-9a8c-1aa4cc4e84e3")
                        .Column(3)
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
                RemotingSessionSummaryPage page = new RemotingSessionSummaryPage(spreadsheet, directory.Entries);
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
                    page.Row(3).AssertValues<string>(Is.Not.Empty);
                    page.Row(4).AssertValues<string>(Is.Empty);

                    page.FindRow(
                        2,
                        value => value == "4682a57e-7d20-4b2c-a2ae-46a6899feaaf")
                        .Column(3)
                        .AssertValue<int>(Is.GreaterThan(1));
                }
            }
        }
    }
}