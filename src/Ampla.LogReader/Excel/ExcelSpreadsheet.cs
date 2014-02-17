using System;
using System.Data;
using System.IO;
using System.Linq;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Excel.Writer;
using ClosedXML.Excel;

namespace Ampla.LogReader.Excel
{
    /// <summary>
    ///     Excel Spreadsheet class that allows reading from and writing to Excel 
    /// </summary>
    public class ExcelSpreadsheet : IExcelSpreadsheet
    {
        private XLWorkbook workbook;
        private readonly string filename;
        private readonly bool existingFile;
        private bool disposed = true;
        private bool isReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelSpreadsheet"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private ExcelSpreadsheet(string filename)
        {
            ArgumentCheck.IsNotNull(filename);
            ArgumentCheck.IsNotEmpty(filename);

            FileInfo fileInfo = new FileInfo(filename);
            existingFile = fileInfo.Exists;

            this.filename = filename;
            workbook = existingFile
                           ? new XLWorkbook(filename, XLEventTracking.Enabled)
                           : new XLWorkbook(XLEventTracking.Enabled);
            disposed = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (workbook == null) return;
            if (disposed) return;
            if (!IsReadOnly)
            {
                if (existingFile)
                {
                    workbook.Save();
                }
                else
                {
                    workbook.SaveAs(filename);
                }
            }
            disposed = true;
            workbook = null;
        }


        /// <summary>
        /// Creates a new Spreadsheet that will be saved as the specified filename
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static IExcelSpreadsheet CreateNew(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            ExcelSpreadsheet excel = new ExcelSpreadsheet(filename);
            return excel;
        }

        /// <summary>
        ///     Opens a new Spreadsheet as ReadOnly.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static IExcelSpreadsheet OpenReadOnly(string filename)
        {
            ExcelSpreadsheet spreadsheet = new ExcelSpreadsheet(filename) { IsReadOnly = true };
            return spreadsheet;
        }


        private void CheckDisposed()
        {
            if (disposed) throw new ObjectDisposedException("ExcelSpreadsheet");
        }

        public IWorksheetWriter WriteDataToWorksheet(DataTable dataTable, string name)
        {
            if (IsReadOnly) throw new InvalidOperationException("Excel Spreadsheet is opened as ReadOnly");

            var worksheet = workbook.Worksheets.Add(dataTable, name);
            
            return new WorksheetWriter(worksheet);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                CheckDisposed();
                return isReadOnly;

            }
            private set { isReadOnly = value; }
        }

        /// <summary>
        /// Gets or creates a new worksheet
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IWorksheet GetWorksheet(string name)
        {
            CheckDisposed();
            var worksheet = GetOrCreateWorksheet(name);
            return new ExcelWorksheet(worksheet, isReadOnly);
        }

        /// <summary>
        ///     Opens a new worksheet for reading.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IWorksheetReader ReadWorksheet(string name)
        {
            CheckDisposed();
            var worksheet = GetOrCreateWorksheet(name);
            return new WorksheetReader(worksheet);
        }

        /// <summary>
        ///     Get a worksheet writer for the specified worksheet
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IWorksheetWriter WriteToWorksheet(string name)
        {
            if (IsReadOnly) throw new InvalidOperationException("Excel Spreadsheet is opened as ReadOnly");

            var worksheet = GetOrCreateWorksheet(name);
            return new WorksheetWriter(worksheet);
        }

        /// <summary>
        /// Gets an existing or creates a new worksheet.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private IXLWorksheet GetOrCreateWorksheet(string name)
        {
            IXLWorksheet worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == name);
            if (worksheet == null)
            {
                if (IsReadOnly)
                {
                    string message = string.Format("Worksheet: '{0}' does not exist.", name);
                    throw new InvalidOperationException(message);
                }

                worksheet = workbook.AddWorksheet(name);
            }
            return worksheet;
        }
    }
}