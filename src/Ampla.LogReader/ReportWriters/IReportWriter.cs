using System;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public interface IReportWriter : IDisposable
    {
        IDisposable StartReport(string reportName);
        void EndReport();

        IDisposable StartSection(string subject);
        void EndSection();

        void Write(Result result);
        void Write(string format, params object[] args);
    }
}