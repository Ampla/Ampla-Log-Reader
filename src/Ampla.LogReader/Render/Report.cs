using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Render
{
    public abstract class Report : IRender
    {
        private readonly List<WcfCall> wcfCalls;
        private readonly TextWriter writer;

        protected Report(List<WcfCall> wcfCalls, TextWriter writer)
        {
            this.wcfCalls = wcfCalls;
            this.writer = writer;
        }

        protected List<WcfCall> WcfCalls
        {
            get { return wcfCalls; }
        }

        public void Render()
        {
            RenderReport(writer);
        }

        protected abstract void RenderReport(TextWriter textWriter);
    }
}