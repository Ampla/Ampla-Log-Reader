using System;
using System.Data;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Excel.Writer;

namespace Ampla.LogReader.Excel
{
    public interface IExcelSpreadsheet : IDisposable
    {
        /// <summary>
        /// Gets or creates a new worksheet
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IWorksheet GetWorksheet(string name);

        /// <summary>
        ///     Opens a new worksheet for reading.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IWorksheetReader ReadWorksheet(string name);

        /// <summary>
        ///     Get a worksheet writer for the specified worksheet
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IWorksheetWriter WriteToWorksheet(string name);

        /// <summary>
        ///     Writes a DataTable as a new Worksheet
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="name"></param>
        IWorksheetWriter WriteDataToWorksheet(DataTable dataTable, string name);

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        bool IsReadOnly { get; }

        /// <summary>
        ///     Gets a list of worksheet names
        /// </summary>
        /// <returns></returns>
        string[] GetWorksheetNames();
    }
}