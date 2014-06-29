using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.Reports.Pages;

namespace Ampla.LogReader.Reports.Packs
{
    public class RemotingExcelReportPack : ReportPack
    {
        private readonly string fileName;
        private readonly ILogReader<RemotingEntry> reader;

        public RemotingExcelReportPack(AmplaProject amplaProject)
        {
            fileName = amplaProject.ProjectName + ".Remoting.xlsx";
            reader = new RemotingDirectory(amplaProject);
        }

        public RemotingExcelReportPack(string fileName, AmplaProject amplaProject)
        {
            this.fileName = fileName;
            reader = new RemotingDirectory(amplaProject);
        }

        public RemotingExcelReportPack(string fileName, ILogReader<RemotingEntry> reader)
        {
            this.fileName = fileName;
            this.reader = reader;
        }

        public override void Render()
        {
            reader.Read();

            using (IExcelSpreadsheet excel = ExcelSpreadsheet.CreateNew(fileName))
            {
                new RemotingSummaryPage(excel, reader.Entries).Render();
                new RemotingSessionSummaryPage(excel, reader.Entries).Render();
                excel.WriteDataToWorksheet(RemotingEntry.CreateDataTable(reader.Entries), "Remoting");
            }
        }
    }
}