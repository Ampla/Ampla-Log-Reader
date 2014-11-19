using System;
using System.Data;
using Ampla.LogReader.Excel;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Reports.Pages;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Packs
{
    public class WcfExcelReportPack : ReportPack
    {
        private readonly string fileName;
        private readonly ILogReader<WcfCall> reader;
        private readonly TimeZoneInfo timeZoneInfo;

        public WcfExcelReportPack(AmplaProject amplaProject)
        {
            fileName = amplaProject.ProjectName + ".Wcf.xlsx";
            reader = new WcfLogDirectory(amplaProject);
            timeZoneInfo = TimeZoneInfo.Local;
        }
        public WcfExcelReportPack(string fileName, AmplaProject amplaProject)
        {
            this.fileName = fileName;
            reader = new WcfLogDirectory(amplaProject);
        }

        public WcfExcelReportPack(string fileName, ILogReader<WcfCall> reader, TimeZoneInfo timeZoneInfo)
        {
            this.fileName = fileName;
            this.reader = reader;
            this.timeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Local;
        }

        public override void Render()
        {
            reader.Read();
            using (IExcelSpreadsheet excel = ExcelSpreadsheet.CreateNew(fileName))
            {
                new WcfSummaryPage(excel, reader.Entries).Render();
                DataTable wcfCalls = new WcfCallTable(timeZoneInfo).Create(reader.Entries);
                excel.WriteDataToWorksheet(wcfCalls, "WcfCalls");

                // write fault details to separate page
                new WcfFaultSummaryPage(excel, reader.Entries).Render();
            }
        }
    }
}