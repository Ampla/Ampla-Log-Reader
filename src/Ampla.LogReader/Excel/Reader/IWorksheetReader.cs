using System;
using System.Collections.Generic;

namespace Ampla.LogReader.Excel.Reader
{
    public interface IWorksheetReader : IDisposable
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
        ///     Reads the current cell and moves to the right
        /// </summary>
        /// <returns></returns>
        string Read();

        /// <summary>
        /// Reads the current cell as the specified type and moves to the right
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ReadValue<T>();

        /// <summary>
        ///     Reads a row of values from the current position and move down a row
        /// </summary>
        /// <returns></returns>
        List<string> ReadRow();

        /// <summary>
        ///     Is the current cell past the end of the data
        /// </summary>
        /// <returns></returns>
        bool IsEndOfData();
    }
}