using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ampla.LogReader.Render;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            TextWriter writer = System.Console.Out;
            WcfLogDirectory directory = new WcfLogDirectory("WebServiceDemo");
            directory.Execute();

            WcfErrorReport report = new WcfErrorReport(directory.WcfCalls, writer);
            report.Render();

            System.Console.WriteLine("Press any key to continue:");
            System.Console.ReadLine();
        }
    }
}
