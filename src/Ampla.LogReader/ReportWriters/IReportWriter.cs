using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public interface IReportWriter
    {
        void NewSubject(string subject);

        void Write(Result result);
        void Write(string format, params object[] args);
    }
}