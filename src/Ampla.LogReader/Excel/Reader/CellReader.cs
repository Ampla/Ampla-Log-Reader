using ClosedXML.Excel;

namespace Ampla.LogReader.Excel.Reader
{
    /// <summary>
    ///     Reader class that will get the current cell
    /// </summary>
    public class CellReader : ICellReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellReader"/> class.
        /// </summary>
        /// <param name="current">The current.</param>
        public CellReader(IXLCell current)
        {
            ArgumentCheck.IsNotNull(current);

            Address = current.Address.ToStringRelative(false);
            Row = current.Address.RowNumber;
            Column = current.Address.ColumnNumber;
            Value = current.Value;
        }

        /// <summary>
        /// Gets the address of the current cell.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the row number of the current cell
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        public int Row { get; private set; }

        /// <summary>
        /// Gets the column number of the current cell
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column { get; private set; }

        /// <summary>
        /// Gets the value of the current cell
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; private set; }
    }
}