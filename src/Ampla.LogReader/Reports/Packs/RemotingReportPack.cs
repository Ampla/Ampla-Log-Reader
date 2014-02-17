using Ampla.LogReader.Excel;
using Ampla.LogReader.Remoting;

namespace Ampla.LogReader.Reports.Packs
{
    public class RemotingReportPack : ReportPack
    {
        private readonly string fileName;
        private readonly ILogReader<RemotingEntry> reader;

        public RemotingReportPack(string fileName, ILogReader<RemotingEntry> reader)
        {
            this.fileName = fileName;
            this.reader = reader;
        }

        public override void Render()
        {
            using (IExcelSpreadsheet excel = ExcelSpreadsheet.CreateNew(fileName))
            {
                excel.WriteDataToWorksheet(RemotingEntry.CreateDataTable(reader.Entries), "Remoting");
            }
        }

    }
}