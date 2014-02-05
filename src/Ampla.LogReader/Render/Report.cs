using System.Collections.Generic;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Render
{
    public abstract class Report : IRender
    {
        private readonly List<WcfCall> wcfCalls;
        private readonly IReportWriter reportWriter;

        protected Report(List<WcfCall> wcfCalls, IReportWriter reportWriter)
        {
            this.wcfCalls = wcfCalls;
            this.reportWriter = reportWriter;
        }

        protected List<WcfCall> WcfCalls
        {
            get { return wcfCalls; }
        }

        public void Render()
        {
            RenderReport(reportWriter);
        }

        protected abstract void RenderReport(IReportWriter reportWriter);
    }
}