using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Packs
{
    public class WcfExcelReportPack : ReportPack
    {
        private readonly string fileName;
        private readonly ILogReader<WcfCall> reader;

        public WcfExcelReportPack(AmplaProject amplaProject)
        {
            fileName = amplaProject.ProjectName + ".Wcf.xlsx";
            reader = new WcfLogDirectory(amplaProject);
        }

        public WcfExcelReportPack(string fileName, ILogReader<WcfCall> reader)
        {
            this.fileName = fileName;
            this.reader = reader;
        }

        public override void Render()
        {
            reader.Read();
            using (IExcelSpreadsheet excel = ExcelSpreadsheet.CreateNew(fileName))
            {
                excel.WriteDataToWorksheet(WcfCall.CreateDataTable(reader.Entries), "WcfCalls");
            }
        }
    }
}