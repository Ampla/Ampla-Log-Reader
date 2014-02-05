using System;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public class TextReportWriter : IReportWriter
    {
        private readonly System.IO.TextWriter textWriter;

        public TextReportWriter(System.IO.TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Write(Result result)
        {
            textWriter.WriteLine("{0}: {1}", result.Topic, result.Data);
        }

        public void NewSubject(string subject)
        {
            string sep = new string('-', Math.Min(subject.Length, 20));
            textWriter.WriteLine(sep);
            textWriter.WriteLine(subject);
            textWriter.WriteLine(sep);
        }

        public void Write(string format, params object[] args)
        {
            textWriter.WriteLine(format, args);
        }
    }
}