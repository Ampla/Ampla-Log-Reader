using Ampla.LogReader.EventLogs;
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
            using (IExcelSpreadsheet excel = ExcelSpreadsheet.CreateNew(fileName))
            {
                foreach (EventLogReader reader in eventLogSystem.GetReaders())
                {
                    reader.Read();
                    excel.WriteDataToWorksheet(SimpleEventLogEntry.CreateDataTable(reader.Entries), reader.Name);
                }
            }
        }

    }
}