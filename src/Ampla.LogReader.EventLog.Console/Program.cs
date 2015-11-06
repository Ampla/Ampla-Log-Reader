using System.IO;
using System.Linq;
using Ampla.LogReader.EventLogs;
using Ampla.LogReader.Reports.Packs;
using CommandLine.Text;

namespace Ampla.LogReader.EventLog.Console
{
    public static class Program
    {
        static void Main(string[] args)
        {
            EventLogReaderOptions options = new EventLogReaderOptions();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                TextWriter writer = System.Console.Out;

                writer.WriteLine("EventLogFile: {0}", options.EventLogFile);
                writer.WriteLine("EventLogDirectory: {0}", options.EventLogDirectory);
                writer.WriteLine("OutputFile: {0}", options.OutputFile);

                bool fileExists = File.Exists(options.EventLogFile);
                bool dirExists = !string.IsNullOrEmpty(options.EventLogDirectory) &&
                                 Directory.Exists(options.EventLogDirectory);
                string[] files = new string[0];
                
                if (dirExists)
                {
                    DirectoryInfo dir = new DirectoryInfo(options.EventLogDirectory);
                    files = dir.EnumerateFiles("*.evtx").Select(fi => fi.FullName).ToArray();
                }
                else if (fileExists)
                {
                    files = new []{options.EventLogFile};
                }
                else
                {
                    writer.WriteLine("Need to specify an event log file or directory\r\n{0}", HelpText.AutoBuild(options));
                }

                if (files.Length > 0)
                {
                    writer.WriteLine("Event Log Processing");
                    foreach (string file in files)
                    {
                        writer.WriteLine(" EventLog: {0}", file);
                    }
                    IEventLogSystem eventLogSystem = new FileEventLogSystem(files);

                    new EventLogReportPack(options.OutputFile, eventLogSystem).Render();
                }
            }

        }

    }
}
