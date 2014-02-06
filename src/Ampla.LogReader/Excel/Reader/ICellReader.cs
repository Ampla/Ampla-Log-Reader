namespace Ampla.LogReader.Excel.Reader
{
    public interface ICellReader
    {
        /// <summary>
        /// Gets the address of the current cell.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        string Address { get; }

        /// <summary>
        /// Gets the row number of the current cell
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        int Row { get; }

        /// <summary>
        /// Gets the column number of the current cell
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        int Column { get; }

        /// <summary>
        /// Gets the value of the current cell
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        object Value { get; }
    }
}