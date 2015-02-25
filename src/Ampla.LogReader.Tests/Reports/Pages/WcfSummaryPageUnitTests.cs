using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Pages
{
    [TestFixture]
    public class WcfSummaryPageUnitTests : ExcelTestFixture
    {
        private const string logFileName = @".\Wcf\Resources\SingleEntry.log";

        [Test]
        public void NoRecords()
        {
            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                var page = CreatePage(spreadsheet, new List<WcfCall>());
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
            ILogReader<WcfCall> logReader = new WcfLogReader(logFileName);
            logReader.ReadAll();

            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                var page = CreatePage(spreadsheet, logReader.Entries);
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
                WcfLogDirectory directory = new WcfLogDirectory(project);
                directory.ReadAll();
                var page = CreatePage(spreadsheet, directory.Entries);
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

        private static ReportPage<WcfCall> CreatePage(IExcelSpreadsheet spreadsheet, List<WcfCall> entries)
        {
            return new WcfSummaryPage(spreadsheet, entries);
        }

    }
}