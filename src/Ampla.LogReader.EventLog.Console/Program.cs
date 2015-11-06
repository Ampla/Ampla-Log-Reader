using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Reports.Packs;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.EventLog.Console
{
    public static class Program
    {
        static void Main(string[] args)
        {
            EventLogReaderOptions options = new EventLogReaderOptions();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                using (IReportWriter reportWriter = GetReportWriter(options))
                {
                    System.Console.WriteLine("EventLogFile: {0}", options.EventLogFile);
                    System.Console.WriteLine("OutputFile: {0}", options.OutputFile);
                    System.Console.WriteLine("TimeZone: {0}", options.TimeZone);

                    TextWriter writer = System.Console.Out;

                    TimeZoneInfo timeZone = TimeZoneHelper.GetTimeZone();
                    if (!string.IsNullOrEmpty(options.TimeZone))
                    {
                        timeZone = TimeZoneHelper.GetSpecificTimeZone(options.TimeZone);
                    }


                    writer.WriteLine("Event Log Processing");
                    IEventLogSystem eventLogSystem = new FileEventLogSystem(options.EventLogFile);

                    new EventLogReportPack("EventLog.Details.xlsx", eventLogSystem).Render();
                }

            }

        }

        private static IReportWriter GetReportWriter(EventLogReaderOptions options)
        {
            string fileName = options.OutputFile ?? "output.xlsx";
            IReportWriter reportWriter = new ExcelReportWriter(fileName);
            return reportWriter;
        }
    }
}
