using System.Collections.Generic;
using ClosedXML.Excel;

namespace Ampla.LogReader.Excel.Reader
{
    /// <summary>
    ///     Allows reading the values from a worksheet
    /// </summary>
    public class WorksheetReader : IWorksheetReader
    {
        private IXLCell current;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorksheetReader"/> class.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        public WorksheetReader(IXLWorksheet worksheet)
        {
            current = worksheet.FirstRow().FirstCell();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            current = null;
        }

        /// <summary>
        /// Reads a row of values from the current position and moves down a row
        /// </summary>
        /// <returns></returns>
        public List<string> ReadRow()
        {
            IXLCell position = current;
            var values = new List<string>();
            string value = current.GetString();

            while (!string.IsNullOrEmpty(value))
            {
                values.Add(value);

                current = current.CellRight();
                value = current.GetString();
            }
            current = position.CellBelow();
            return values;
        }

        /// <summary>
        /// Is the current cell past the end of the data
        /// </summary>
        /// <returns></returns>
        public bool IsEndOfData()
        {
            IXLCell lastCell = current.Worksheet.LastCellUsed();
            if (lastCell == null)
            {
                return true;
            }

            int lastRow = lastCell.Address.RowNumber;
            int currentRow = current.Address.RowNumber;

            if (lastRow < currentRow)
            {
                return true;
            }

            if (lastRow == currentRow)
            {
                return current.Address.ColumnNumber > lastCell.Address.ColumnNumber;
            }
            return false;
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
        /// Reads the current cell and moves to the right
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            string value = current.GetString();

            current = current.CellRight();
            return value;
        }

        /// <summary>
        /// Reads the current cell as the specified type and moves to the right
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadValue<T>()
        {
            T value = current.GetValue<T>();
            current = current.CellRight();
            return value;
        }

        /// <summary>
        /// Gets the current cell
        /// </summary>
        /// <returns></returns>
        public ICellReader GetCurrentCell()
        {
            return new CellReader(current);
        }
    }
}