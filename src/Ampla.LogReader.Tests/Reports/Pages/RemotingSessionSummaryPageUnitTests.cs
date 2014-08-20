using System;
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

            DateTime start = logReader.Entries[0].CallTimeLocal;
            DateTime end = logReader.Entries[0].CallTimeLocal;

            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.OpenReadOnly(Filename))
            {
                using (ExcelPage page = new ExcelPage(spreadsheet, pageName))
                {
                    page.ReadLines(10);
                    page.Row(1).AssertValues<string>(Is.Not.Empty);
                    page.Row(2).AssertValues<string>(Is.Not.Empty);
                    page.Row(3).AssertValues<string>(Is.Not.Empty);

                    ExcelRow row = page.FindRow(
                        2,
                        value => value == "9f061bbe-a80d-4881-9a8c-1aa4cc4e84e3");
                    row.Column(3).AssertValue<int>(Is.EqualTo(1));
                    row.Column(4).AssertValue<DateTime>(Is.EqualTo(start));
                    row.Column(5).AssertValue<DateTime>(Is.EqualTo(end));
                }
            }
        }

        [Test]
        public void MultipleRecords()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();

            DateTime start;
            DateTime end = DateTime.MinValue;
            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                RemotingDirectory directory = new RemotingDirectory(project);
                directory.Read();
                RemotingSessionSummaryPage page = new RemotingSessionSummaryPage(spreadsheet, directory.Entries);
                page.Render();
                pageName = page.PageName;


                start = directory.Entries[0].CallTimeLocal;

                foreach (var entry in directory.Entries)
                {
                    if (entry.Arguments.Length > 0 
                        && entry.Arguments[0].Value == "4682a57e-7d20-4b2c-a2ae-46a6899feaaf"
                        && entry.CallTimeLocal > end)
                    {
                        end = entry.CallTimeLocal;
                    }
                }
                
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

                    ExcelRow row = page.FindRow(
                       2,
                       value => value == "4682a57e-7d20-4b2c-a2ae-46a6899feaaf");
                    row.Column(3).AssertValue<int>(Is.GreaterThan(1));
                    row.Column(4).AssertValue<DateTime>(Is.EqualTo(start));
                    row.Column(5).AssertValue<DateTime>(Is.EqualTo(end));

                    page.Row(2).Column(7).AssertValue<string>(Is.EqualTo("Offset"));
                    double delta = TimeSpan.FromSeconds(5).TotalDays;
                    double expectedOffset = start.TimeOfDay.TotalDays;
                    row.Column(7).AssertValue<double>(Is.InRange(expectedOffset - delta, expectedOffset + delta));

                }
            }
        }
    }
}