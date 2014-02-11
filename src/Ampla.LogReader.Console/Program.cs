using System.Diagnostics;
using System.IO;
using System.Xml;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Reports;
using Ampla.LogReader.Reports.EventLogs;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            LogReaderOptions options = new LogReaderOptions();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                if (options.Debug)
                {
                    Debugger.Break();
                }

                using (IReportWriter reportWriter = GetReportWriter(options))
                {
                    System.Console.WriteLine("Project: {0}", options.Project);
                    System.Console.WriteLine("Output: {0}", options.OutputMode);

                    TextWriter writer = System.Console.Out;

                    if (options.LogDirectory != null)
                    {
                        AmplaProject project = new AmplaProject
                            {
                                ProjectName = "",
                                Directory = options.LogDirectory,
                            };

                        if (!options.SkipWcf)
                        {
                            writer.WriteLine("LogDirectory: {0}", project.Directory);
                            WcfLogDirectory directory = new WcfLogDirectory(project);
                            directory.Read();

                            writer.WriteLine("Read {0} entries from WcfLog files", directory.WcfCalls.Count);

                            new WcfSummaryReport(
                                directory.WcfCalls, reportWriter).Render();
                            new WcfFaultSummaryReport(
                                directory.WcfCalls, reportWriter).Render();
                            new WcfHourlySummaryReport(
                                directory.WcfCalls, reportWriter).Render();
                            new WcfUrlSummaryReport(
                                directory.WcfCalls, reportWriter).Render();
                            new WcfActionSummaryReport(
                                directory.WcfCalls, reportWriter).Render();
                        }

                        if (!options.SkipEventLog)
                        {
                            writer.WriteLine("Event Log Processing");
                            EventLogSystem eventLogSystem = new EventLogSystem();

                            foreach (EventLog eventLog in eventLogSystem.GetEventLogs())
                            {
                                IEventLogReader reader = new EventLogReader(eventLog);
                                reader.Read();

                                new EventLogSummaryReport(eventLog, reader.EventLogEntries, reportWriter).Render();
                                new EventLogHourlySummaryReport(eventLog, reader.EventLogEntries, reportWriter).Render();
                                new EventLogDumpReport(eventLog, reader.EventLogEntries, reportWriter).Render();
                            }
                        }
                    }
                    else
                    {
                        AmplaProjectDirectories projectDirectories = new AmplaProjectDirectories();
                        foreach (AmplaProject project in projectDirectories.Projects)
                        {
                            writer.WriteLine("======================");
                            writer.WriteLine("Project: {0}", project.ProjectName);
                            writer.WriteLine("Directory: {0}", project.Directory);

                            WcfLogDirectory directory = new WcfLogDirectory(project);
                            directory.Read();

                            WcfSummaryReport statistics = new WcfSummaryReport(directory.WcfCalls,
                                                                               reportWriter);
                            statistics.Render();
                        }
                    }
                }
            }
         
        }

        private static IReportWriter GetReportWriter(LogReaderOptions options)
        {
            IReportWriter reportWriter;
            switch (options.OutputMode)
            {
                case OutputMode.Text:
                    {
                        reportWriter = new TextReportWriter(System.Console.Out);
                        break;
                    }
                case OutputMode.Xml:
                    {
                        string fileName = options.OutputFile ?? "output.xml";
                        XmlWriterSettings settings = new XmlWriterSettings() {Indent = true};
                        XmlWriter xmlWriter = XmlWriter.Create(fileName, settings);
                        reportWriter = new XmlReportWriter(xmlWriter);
                        break;
                    }
                case OutputMode.Excel:
                    {
                        string fileName = options.OutputFile ?? "output.xlsx";
                        reportWriter = new ExcelReportWriter(fileName);
                        break;
                    }
                default:
                    {
                        reportWriter = new TextReportWriter(System.Console.Out);
                        break;
                    }
            }
            return reportWriter;
        }
    }
}
