using System.Collections.Generic;
using System.Linq;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports
{
    public class WcfErrorReport : Report
    {
        public WcfErrorReport(List<WcfCall> wcfCalls, IReportWriter reportWriter) : base(wcfCalls, reportWriter)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            reportWriter.NewSubject("WcfErrors");

            int errors = 0;

            int entries = WcfCalls.Count();

            foreach (var call in WcfCalls.Where(call => call.IsFault))
            {
                errors++;
                reportWriter.Write("{0} - {1}", call.CallTime, call.FaultMessage);
            }

            reportWriter.Write("{0} entries with {1} errors ({2:0.00}%).", entries, errors, errors * 100.0D / entries);
        }
    }
}