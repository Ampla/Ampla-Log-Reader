using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Render
{
    public class WcfErrorReport : Report
    {
        public WcfErrorReport(List<WcfCall> wcfCalls, TextWriter writer) : base(wcfCalls, writer)
        {
        }

        protected override void RenderReport(TextWriter textWriter)
        {
            textWriter.WriteLine("WcfErrors");

            int errors = 0;

            int entries = WcfCalls.Count();

            foreach (var call in WcfCalls.Where(call => call.IsFault))
            {
                errors++;
                textWriter.WriteLine(call.CallTime);
                //textWriter.WriteLine(call.RequestMessage);
                textWriter.WriteLine(call.FaultMessage);
            }

            textWriter.WriteLine("{0} entries with {1} errors ({2:0.00}%).", entries, errors, errors * 100.0D / entries);
        }
    }
}