using CommandLine;

namespace Ampla.LogReader.EventLog.Console
{
    public class EventLogReaderOptions
    {
        [Option('o', "output", Required = false, HelpText = "Select the Output file", DefaultValue = "EventLog.Report.xslx")]
        public string OutputFile { get; set; }

        [Option('f', "file", Required = false, HelpText = "Event Log file to parse", DefaultValue = "EventLog.evtx")]
        public string EventLogFile { get; set; }

        [Option('d', "dir", Required = false, HelpText = "Event Log directory to parse for Event Log files (*.evtx)", DefaultValue = null)]
        public string EventLogDirectory { get; set; }

//        [Option('z', "timezone", HelpText = "TimeZone for local date times", DefaultValue = null)]
//        public string TimeZone { get; set; }
    }
}