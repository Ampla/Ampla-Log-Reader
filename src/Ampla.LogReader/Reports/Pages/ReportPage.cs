﻿using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.Reports.Pages
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
            
            RenderPage(writer);
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

        protected void UpdateStatistic(IStatistic<TEntry> statistic)
        {
            foreach (TEntry entry in entries)
            {
                statistic.Add(entry);
            }
        }

        public string PageName { get; private set; }

        protected abstract void RenderPage(IWorksheetWriter writer);


        protected void WriteStatistics(TopNStatistics<TEntry> topNStatistics, IWorksheetWriter writer)
        {
            writer.WriteRow(topNStatistics.Name, "Count");
            foreach (var result in topNStatistics.Results)
            {
                writer.WriteRow(result.Topic, (IConvertible)result.Data);
            }
        }
    }
}