using System.Collections.Generic;
using Ampla.LogReader.Excel.Reader;
using ClosedXML.Excel;

namespace Ampla.LogReader.Excel.Writer
{

    /// <summary>
    ///     Allows the writing of values to an Excel Spreadsheet
    /// </summary>
    public class WorksheetWriter : IWorksheetWriter
    {
        private IXLCell current;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorksheetWriter"/> class.
        /// </summary>
        /// <param name="xlWorksheet">The xl worksheet.</param>
        public WorksheetWriter(IXLWorksheet xlWorksheet)
        {
            ArgumentCheck.IsNotNull(xlWorksheet);
            current = xlWorksheet.Cell(1, 1);
        }

        /// <summary>
        /// Gets the current cell
        /// </summary>
        /// <returns></returns>
        public ICellReader GetCurrentCell()
        {
            return new CellReader(current);
        }

        /// <summary>
        /// Moves to the specified cell using the address
        /// </summary>
        /// <param name="address"></param>
        public void MoveTo(string address)
        {
            current = current.Worksheet.Cell(address);
        }

        /// <summary>
        /// Moves to the specified row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void MoveTo(int row, int column)
        {
            current = current.Worksheet.Cell(row, column);
        }

        /// <summary>
        /// Writes a list of values to the current row
        /// </summary>
        /// <param name="row"></param>
        public void WriteRow(List<string> row)
        {
            IXLCell start = current;
            foreach (string value in row)
            {
                current.Value = value;
                current = current.CellRight();
            }
            current = start.CellBelow();
        }

        /// <summary>
        /// Writes the specified value to the current cell and moves to the next column
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(string value)
        {
            current.Value = value;
            current = current.CellRight();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            current = null;
        }
    }
}