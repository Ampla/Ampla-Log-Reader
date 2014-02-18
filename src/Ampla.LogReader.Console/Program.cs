using System.IO;
using System.Xml;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.Remoting;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Reports.Packs;
using Ampla.LogReader.Reports.Remoting;
using Ampla.LogReader.Reports.Wcf;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Console
{
    public static class Program
    {
        static void Main(string[] args)
        {
            LogReaderOptions options = new LogReaderOptions();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                using (IReportWriter reportWriter = GetReportWriter(options))
                {
                    System.Console.WriteLine("Project: {0}", options.Project);
                    System.Console.WriteLine("Output: {0}", options.OutputMode);

                    TextWriter writer = System.Console.Out;

                    if (options.LogDirectory != null)
                    {
                        AmplaProject project = new AmplaProject
                            {
                                ProjectName = "(Custom)",
                                Directory = options.LogDirectory,
                            };

                        if (!options.SkipWcf)
                        {
                            writer.WriteLine("LogDirectory: {0}", project.Directory);
                            WcfLogDirectory directory = new WcfLogDirectory(project);
                            directory.Read();

                            writer.WriteLine("Read {0} entries from WcfLog files", directory.Entries.Count);

                            writer.WriteLine("Creating Wcf Report -> WcfCall.Details.xlsx");

                            new WcfExcelReportPack("WcfCall.Details.xlsx", directory).Render();

                            new WcfSummaryReport(
                                directory.Entries, reportWriter).Render();
                            new WcfFaultSummaryReport(
                                directory.Entries, reportWriter).Render();
                            new WcfHourlySummaryReport(
                                directory.Entries, reportWriter).Render();
                            new WcfUrlSummaryReport(
                                directory.Entries, reportWriter).Render();
                            new WcfActionSummaryReport(
                                directory.Entries, reportWriter).Render();
                        }

                        if (!options.SkipRemoting)
                        {
                            writer.WriteLine("LogDirectory: {0}", project.Directory);
                            RemotingDirectory directory = new RemotingDirectory(project);
                            directory.Read();

                            writer.WriteLine("Read {0} entries from Remoting files", directory.Entries.Count);

                            new RemotingReportPack("Remoting.Details.xlsx", directory).Render();


                            new RemotingSummaryReport(
                                directory.Entries, reportWriter).Render();
                            new RemotingIdentitySummaryReport(
                                directory.Entries, reportWriter).Render();
                            //new WcfFaultSummaryReport(
                            //    directory.Entries, reportWriter).Render();
                            //new RemotingHourlySummaryReport(
                            //    directory.Entries, reportWriter).Render();
                            //new RemotingUrlSummaryReport(
                            //    directory.Entries, reportWriter).Render();
                            //new WcfActionSummaryReport(
                            //    directory.Entries, reportWriter).Render();
                        }

                        if (!options.SkipEventLog)
                        {
                            writer.WriteLine("Event Log Processing");
                            EventLogSystem eventLogSystem = new EventLogSystem();

                            new EventLogReportPack("EventLog.Details.xlsx", eventLogSystem).Render();
                        }
                    }
                    else
                    {
                        AmplaProjectDirectories projectDirectories = new AmplaProjectDirectories();

                        foreach (AmplaProject project in projectDirectories.GetAmplaProjects())
                        {
                            writer.WriteLine("Project: {0}", project.ProjectName);
                            new WcfExcelReportPack(project).Render();
                            new RemotingReportPack(project).Render();
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
                        XmlWriterSettings settings = new XmlWriterSettings {Indent = true};
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
