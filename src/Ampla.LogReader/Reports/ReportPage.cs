using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports
{
    public abstract class ReportPage<TEntry>
    {
        private readonly IExcelSpreadsheet excelSpreadsheet;
        private readonly List<TEntry> entries;

        protected ReportPage(IExcelSpreadsheet excelSpreadsheet, List<TEntry> entries, string name)
        {
            this.excelSpreadsheet = excelSpreadsheet;
            this.entries = entries;
            PageName = name;
        }

        public void Render()
        {
            IWorksheetWriter writer = excelSpreadsheet.WriteToWorksheet(PageName);
            
            RenderPage(writer, entries);
        }

        protected void UpdateStatistics(IStatistic<TEntry>[] statistics)
        {
            foreach (TEntry entry in entries)
            {
                foreach (IStatistic<TEntry> statistic in statistics)
                {
                    statistic.Add(entry);
                }
            }
        }

        public string PageName { get; private set; }

        protected abstract void RenderPage(IWorksheetWriter writer, IEnumerable<TEntry> list);
    }
}