using System;
using System.Text;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public class SimpleReportWriter : IReportWriter
    {
        private StringBuilder stringBuilder;

        public SimpleReportWriter()
        {
            this.stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("SimpleReportWriter");
        }

        private class DisposeFunc : IDisposable
        {
            private readonly Action action;
            private bool disposed;

            public DisposeFunc(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                if (disposed) return;
                disposed = true;
                action();
            }
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public void Dispose()
        {
        }

        private void WriteMessage(string operation)
        {
            stringBuilder.AppendLine(operation);
        }

        private void WriteMessage(string operation, string arg)
        {
            stringBuilder.AppendLine(operation + " -> " + arg);
        }

        public IDisposable StartReport(string reportName)
        {
            WriteMessage("StartReport", reportName);
            return new DisposeFunc(((IReportWriter)this).EndReport);
        }

        public void EndReport()
        {
            WriteMessage("EndReport");
        }

        public IDisposable StartSection(string section)
        {
            WriteMessage("StartSection", section);
            return new DisposeFunc(((IReportWriter)this).EndSection);
        }

        public void EndSection()
        {
            WriteMessage("EndSection");
        }

        public void Write(Result result)
        {
            WriteMessage(result.Topic, result.Data.ToString());
        }

        public void Write(string format, params object[] args)
        {
            WriteMessage("Write", string.Format(format, args));
        }
    }
}