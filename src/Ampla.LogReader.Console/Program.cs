using System.IO;
using Ampla.LogReader.FileSystem;
using Ampla.LogReader.ReportWriters;
using Ampla.LogReader.Reports;

namespace Ampla.LogReader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            LogReaderOptions options = new LogReaderOptions();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                IReportWriter reportWriter = new TextReportWriter(System.Console.Out);
 
                System.Console.WriteLine("Project: {0}", options.Project);

                TextWriter writer = System.Console.Out;

                if (options.LogDirectory != null)
                {
                    AmplaProject project = new AmplaProject
                        {
                            ProjectName = "",
                            Directory = options.LogDirectory,
                        };

                    writer.WriteLine("LogDirectory: {0}", project.Directory);
                    WcfLogDirectory directory = new WcfLogDirectory(project);
                    directory.Read();

                    writer.WriteLine("Read {0} entries from WcfLog files", directory.WcfCalls.Count);
                    
                    WcfSummaryReport statistics = new WcfSummaryReport(directory.WcfCalls, reportWriter);
                    statistics.Render();

                    //WcfFaultSummaryReport report = new WcfFaultSummaryReport(directory.WcfCalls, reportWriter);
                    //report.Render();

                    WcfHourlySummaryReport hourlyReport = new WcfHourlySummaryReport(directory.WcfCalls, reportWriter);
                    hourlyReport.Render();
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
          
            System.Console.WriteLine("Press any key to continue:");
            System.Console.ReadLine();

        }
    }
}
