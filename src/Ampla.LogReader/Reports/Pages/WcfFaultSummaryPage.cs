using System.Collections.Generic;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Statistics;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Pages
{
    public class WcfFaultSummaryPage : ReportPage<WcfCall>
    {
        private readonly WcfSummaryStatistic summaryStatistic;
        private readonly TopNStatistics<WcfCall> topNFaults;

        public WcfFaultSummaryPage(IExcelSpreadsheet excelSpreadsheet, List<WcfCall> entries)
            : base(excelSpreadsheet, entries, "Faults")
        {
            summaryStatistic = new WcfSummaryStatistic("Faults");
            topNFaults = new TopNStatistics<WcfCall>
                ("Top 20 Faults", 20,
                 entry => entry.FaultMessage,
                 entry => entry.IsFault);
        }

        protected override void RenderPage(IWorksheetWriter writer)
        {
            UpdateStatistics(new IStatistic<WcfCall>[] {summaryStatistic, topNFaults});

            writer.WriteRow("Summary of Wcf Faults");
            if (summaryStatistic.Errors > 0)
            {
                writer.WriteRow("Number of Faults:", summaryStatistic.Errors);
                writer.WriteRow("Fault (%):", summaryStatistic.ErrorPercent);

                writer.WriteRow();

                WriteStatistics(topNFaults, writer);
            }
            else
            {
                writer.WriteRow("No Faults found");
            }
        }
        
    }
}