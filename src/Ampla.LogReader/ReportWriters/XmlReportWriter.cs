using System;
using System.Collections.Generic;
using System.Xml;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.ReportWriters
{
    public class XmlReportWriter : IReportWriter
    {
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
        
        private XmlWriter xmlWriter;
        private readonly Stack<string> depthStack = new Stack<string>();  

        public XmlReportWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Reports");
        }

        public IDisposable StartReport(string reportName)
        {
            xmlWriter.WriteComment(reportName);
            xmlWriter.WriteStartElement("Report");
            xmlWriter.WriteAttributeString("name", reportName);
            depthStack.Push("Report");
            return new DisposeFunc(((IReportWriter)this).EndReport);
        }

        void IReportWriter.EndReport()
        {
            string top = depthStack.Pop();
            xmlWriter.WriteEndElement();
            if (top != "Report")
            {
                throw new InvalidOperationException("Expected Stack to be 'Report' but was: " + top);
            }
        }

        public IDisposable StartSection(string subject)
        {
            xmlWriter.WriteStartElement("Section");
            xmlWriter.WriteAttributeString("name", subject);
            depthStack.Push("Section");
            xmlWriter.WriteComment(subject);
            return new DisposeFunc(((IReportWriter)this).EndSection);
        }

        void IReportWriter.EndSection()
        {
            string top = depthStack.Pop();
            xmlWriter.WriteEndElement();
            if (top != "Section")
            {
                throw new InvalidOperationException("Expected Stack to be 'Section' but was: " + top);
            }
        }

        public void Write(Result result)
        {
            xmlWriter.WriteStartElement("Result");
            xmlWriter.WriteAttributeString("topic", result.Topic);
            xmlWriter.WriteAttributeString("value", result.Data.ToString());
            xmlWriter.WriteEndElement();
        }

        public void Write(string format, params object[] args)
        {
            xmlWriter.WriteComment(string.Format(format, args));
        }

        public void Dispose()
        {
            if (xmlWriter != null)
            {
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                xmlWriter = null;
            }
        }
    }
}