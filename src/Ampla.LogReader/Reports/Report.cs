using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;

namespace Ampla.LogReader.Reports
{
    public abstract class Report<TEntry> : IRender
    {
        private readonly List<TEntry> entries;
        private readonly IReportWriter reportWriter;

        protected Report(List<TEntry> entries, IReportWriter reportWriter)
        {
            this.entries = entries;
            this.reportWriter = reportWriter;
        }

        protected List<TEntry> Entries
        {
            get { return entries; }
        }

        public void Render()
        {
            RenderReport(reportWriter);
        }

        protected abstract void RenderReport(IReportWriter reportWriter);
    }
}