using System;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Excel.Writer;
using ClosedXML.Excel;

namespace Ampla.LogReader.Excel
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelWorksheet : IWorksheet
    {
        private IXLWorksheet worksheet;
        private readonly bool readOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWorksheet"/> class.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        public ExcelWorksheet(IXLWorksheet worksheet, bool readOnly)
        {
            this.worksheet = worksheet;
            this.readOnly = readOnly;
        }

        /// <summary>
        ///     Open the worksheet for Reading
        /// </summary>
        /// <returns></returns>
        public IWorksheetReader Read()
        {
            return new WorksheetReader(worksheet);
        }

        /// <summary>
        ///     Open the worksheet for Writing
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Worksheet is read only</exception>
        public IWorksheetWriter Write()
        {
            if (readOnly) throw new InvalidOperationException("Worksheet is read only");
            return new WorksheetWriter(worksheet);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            worksheet = null;
        }
    }
}