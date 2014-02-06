using System;
using System.Collections.Generic;
using Ampla.LogReader.Excel.Reader;

namespace Ampla.LogReader.Excel.Writer
{
    /// <summary>
    /// Allows the writing to an Excel Worksheet
    /// </summary>
    public interface IWorksheetWriter : IDisposable
    {

        /// <summary>
        ///     Gets the current cell
        /// </summary>
        /// <returns></returns>
        ICellReader GetCurrentCell();

        /// <summary>
        ///     Moves to the specified cell using the address
        /// </summary>
        /// <param name="address"></param>
        void MoveTo(string address);

        /// <summary>
        ///     Moves to the specified row and column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void MoveTo(int row, int column);

        /// <summary>
        ///     Writes a list of values to the current row
        /// </summary>
        /// <param name="row"></param>
        void WriteRow(List<string> row);

        /// <summary>
        /// Writes the specified value to the current cell and moves to the next column
        /// </summary>
        /// <param name="value">The value.</param>
        void Write(string value);
    }
}