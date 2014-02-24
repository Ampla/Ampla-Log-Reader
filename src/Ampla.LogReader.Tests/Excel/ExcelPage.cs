using System;
using System.Collections.Generic;
using System.Text;
using Ampla.LogReader.Excel.Reader;
using NUnit.Framework;

namespace Ampla.LogReader.Excel
{
    public class ExcelPage : IDisposable
    {
        private IExcelSpreadsheet spreadsheet;
        private readonly string pageName;
        private List<ExcelRow> rows;

        public ExcelPage(IExcelSpreadsheet spreadsheet, string pageName)
        {
            this.spreadsheet = spreadsheet;
            this.pageName = pageName;
            rows = new List<ExcelRow>();
        }

        public void ReadLines(int lines)
        {
            using (IWorksheetReader reader = spreadsheet.ReadWorksheet(pageName))
            {
                for (int i = 0; i < lines; i++)
                {
                    ExcelRow row = new ExcelRow(reader);
                    rows.Add(row);
                }
            }
        }

        public ExcelRow FindRow(int column, Func<string, bool> selectFunc)
        {
            foreach (ExcelRow row in rows)
            {
                string value = row.GetValueAt<string>(column);
                if (value != null && selectFunc(value))
                {
                    return row;
                }
            }
            Assert.Fail("Unable to find a row in column {0}\r\nContents:\r\n{1}", column, this);
            return null;
        }

        public void Dispose()
        {
            rows = null;
            spreadsheet = null;
        }

        public override string ToString()
        {
            if (rows == null)
            {
                return base.ToString() + " {Disposed}";
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Excel Page: {0}", pageName);
            builder.AppendLine();
            foreach (ExcelRow row in rows)
            {
                builder.AppendLine(row.ToString());
            }
            return builder.ToString();
        }

        public ExcelRow Row(int row)
        {
            if (row > 0 && row <= rows.Count)
            {
                ExcelRow excelRow = rows[row - 1];
                return excelRow;
            }
            Assert.Fail("Row {0} was not read in ReadLines().\r\n{1}", row, this);
            return null;
        }
    }
}