using System;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public class TextReportWriter : IReportWriter
    {
        private class DisposeFunc : IDisposable
        {
            private readonly Action action;
            private bool disposed = false;

            public DisposeFunc(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                if (!disposed)
                {
                    disposed = true;
                    action();
                }
            }
        }

        private System.IO.TextWriter textWriter;

        public TextReportWriter(System.IO.TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public IDisposable StartReport(string reportName)
        {
            WriteBlock(reportName, '*');
            return new DisposeFunc(((IReportWriter) this).EndReport);
        }

        void IReportWriter.EndReport()
        {
            textWriter.WriteLine();
        }

        public IDisposable StartSection(string subject)
        {
            WriteBlock(subject, '-');
            return new DisposeFunc(((IReportWriter)this).EndSection);
        }

        void IReportWriter.EndSection()
        {
            textWriter.WriteLine();
        }

        public void Write(Result result)
        {
            textWriter.WriteLine("{0}: {1}", result.Topic, result.Data);
        }

        private void WriteBlock(string text, char mark)
        {
            string sep = new string(mark, Math.Min(text.Length, 20));
            textWriter.WriteLine(sep);
            textWriter.WriteLine(text);
            textWriter.WriteLine(sep);
        }

        public void Write(string format, params object[] args)
        {
            textWriter.WriteLine(format, args);
        }

        public void Dispose()
        {
            if (textWriter != null)
            {
                textWriter.Flush();
            }
            textWriter = null;
        }
    }
}