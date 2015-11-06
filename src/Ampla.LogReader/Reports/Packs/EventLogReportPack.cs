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

                foreach (var reader in eventLogSystem.GetReaders())
                {
                    reader.ReadAll();
                    summaryTable.Add(reader);
                    dataTable.Add(SimpleEventLogEntry.CreateDataTable(reader.Entries));
                    names.Add(reader.Name);
                }

                bool hasDuplicates = false;
                foreach (string name in names)
                {
                    hasDuplicates = names.FindAll(s => s == name).Count > 1;
                }

                if (hasDuplicates)
                {
                    List<string> newNames = new List<string>();
                    foreach (string name in names)
                    {
                        if (!newNames.Contains(name))
                        {
                            newNames.Add(name);
                        }
                        else
                        {
                            int count = 1;
                            string newName = string.Format("{0}.{1}", name, count);
                            while (newNames.Contains(newName))
                            {
                                count++;
                                newName = string.Format("{0}.{1}", name, count);
                            }
                            newNames.Add(newName);
                        }
                    }
                    names = newNames;
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