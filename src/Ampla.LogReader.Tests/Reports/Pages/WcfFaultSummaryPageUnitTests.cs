using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Pages
{
    [TestFixture]
    public class WcfFaultSummaryPageUnitTests : ExcelTestFixture
    {
        private const string logFileName = @".\Wcf\Resources\SingleEntry.log";

        [Test]
        public void EmptyRecords()
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

                    page.Row(2).AssertValues<string>(Is.EquivalentTo(new[] {"No Faults found"}));
                }
            }
        }

        [Test]
        public void NoFaults()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();

            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                WcfLogDirectory directory = new WcfLogDirectory(project);
                directory.Read();
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

                    page.Row(2).AssertValues<string>(Is.EquivalentTo(new[] { "No Faults found" }));
                }
            }
        }

        [Test]
        public void SingleRecordNoFault()
        {
            ILogReader<WcfCall> logReader = new WcfLogReader(logFileName);
            logReader.Read();

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

                    page.Row(2).AssertValues<string>(Is.EquivalentTo(new[] { "No Faults found" }));
                }
            }
        }

        [Test]
        public void MultipleFaults()
        {
            AmplaProject project = AmplaTestProjects.GetWcfFaultsProject();

            string pageName;
            using (IExcelSpreadsheet spreadsheet = ExcelSpreadsheet.CreateNew(Filename))
            {
                WcfLogDirectory directory = new WcfLogDirectory(project);
                directory.Read();
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
                        value => value.StartsWith("Number of Faults"))
                        .Column(2)
                        .AssertValue<int>(Is.GreaterThan(0));
                }
            }
        }

        private static ReportPage<WcfCall> CreatePage(IExcelSpreadsheet spreadsheet, List<WcfCall> entries)
        {
            return new WcfFaultSummaryPage(spreadsheet, entries);
        }

    }
}