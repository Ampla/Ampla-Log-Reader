using System;
using Ampla.LogReader.Excel.Reader;
using Ampla.LogReader.Excel.Writer;

namespace Ampla.LogReader.Excel
{
    /// <summary>
    ///     Excel worksheet that allows either reading or writing
    /// </summary>
    public interface IWorksheet : IDisposable
    {
        /// <summary>
        ///     Open the worksheet for Reading
        /// </summary>
        /// <returns></returns>
        IWorksheetReader Read();

        /// <summary>
        ///     Open the worksheet for Writing
        /// </summary>
        /// <returns></returns>
        IWorksheetWriter Write();
    }
}