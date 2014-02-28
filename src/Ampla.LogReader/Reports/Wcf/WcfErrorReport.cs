using System.Collections.Generic;
using System.Linq;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Wcf
{
    public class WcfErrorReport : Report<WcfCall>
    {
        public WcfErrorReport(List<WcfCall> entries, IReportWriter reportWriter) : base(entries, reportWriter)
        {
        }

        protected override void RenderReport(IReportWriter reportWriter)
        {
            using (reportWriter.StartReport("Wcf Error Report"))
            {

                int errors = 0;

                int entries = Entries.Count();

                foreach (var call in Entries.Where(call => call.IsFault))
                {
                    errors++;
                    reportWriter.Write("{0} - {1}", call.CallTime, call.Fault.FaultString);
                }

                reportWriter.Write("{0} entries with {1} errors ({2:0.00}%).", entries, errors, errors*100.0D/entries);
            }
        }
    }
}