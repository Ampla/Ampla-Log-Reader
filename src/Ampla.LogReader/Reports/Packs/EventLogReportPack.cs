using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.EventLogs.Statistics;
using Ampla.LogReader.Excel;

namespace Ampla.LogReader.Reports.Packs
{
    public class EventLogReportPack : ReportPack
    {
        private readonly string fileName;
        private readonly IEventLogSystem eventLogSystem;

        public EventLogReportPack(string fileName, IEventLogSystem eventLogSystem)
        {
            this.fileName = fileName;
            this.eventLogSystem = eventLogSystem;
        }

        public override void Render()
        {
            List<DataTable> dataTable = new List<DataTable>();
            List<string> names = new List<string>();
            using (IExcelSpreadsheet excel = ExcelSpreadsheet.CreateNew(fileName))
            {
                EventLogSummaryTable summaryTable = new EventLogSummaryTable("Summary");

                foreach (EventLogReader reader in eventLogSystem.GetReaders())
                {
                    reader.Read();
                    summaryTable.Add(reader);
                    dataTable.Add(SimpleEventLogEntry.CreateDataTable(reader.Entries));
                    names.Add(reader.Name);
                }

                excel.WriteDataToWorksheet(summaryTable.GetData(), "Summary");
                for (int i = 0; i < names.Count; i++)
                {
                    excel.WriteDataToWorksheet(dataTable[i], names[i]);
                }
            }
        }

    }
}