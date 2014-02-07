using System;
using Ampla.LogReader.Excel;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Excel.Writer;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public class ExcelReportWriter : IReportWriter
    {
        private IExcelSpreadsheet spreadsheet;
        private IWorksheetWriter excelWriter;

        private class DisposeFunc : IDisposable
        {
            private readonly Action action;
            private bool disposed = false;

            public DisposeFunc(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    disposed = true;
                    action();
                }
            }
        }

        public ExcelReportWriter(string fileName)
        {
            spreadsheet = ExcelSpreadsheet.CreateNew(fileName);
        }

        public IDisposable StartReport(string reportName)
        {
            excelWriter = spreadsheet.WriteToWorksheet(reportName);
            return new DisposeFunc(((IReportWriter) this).EndReport);
        }

        void IReportWriter.EndReport()
        {
            excelWriter = null;
        }

        public IDisposable StartSection(string subject)
        {
            MoveToNextLine();
            excelWriter.Write(subject);
            return new DisposeFunc(((IReportWriter)this).EndSection);
        }

        private void MoveToNextLine()
        {
            ICellReader cell = excelWriter.GetCurrentCell();
            excelWriter.MoveTo(cell.Row + 1, 1);
        }

        void IReportWriter.EndSection()
        {
        }

        public void Write(Result result)
        {
            excelWriter.Write(result.Data.ToString());
        }

        public void Write(string format, params object[] args)
        {
            excelWriter.Write(string.Format(format, args));
            //MoveToNextLine();
        }

        public void Dispose()
        {
            if (excelWriter != null)
            {
                excelWriter.Dispose();
                excelWriter = null;
            }
            if (spreadsheet != null)
            {
                spreadsheet.Dispose();
                spreadsheet = null;
            }
        }
    }
}