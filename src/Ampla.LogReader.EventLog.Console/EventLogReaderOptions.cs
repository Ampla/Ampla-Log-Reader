using CommandLine;

namespace Ampla.LogReader.EventLog.Console
{
    public class EventLogReaderOptions
    {
        [Option('o', "output", Required = false, HelpText = "Select the Output file", DefaultValue = "EventLog.Details.xslx")]
        public string OutputFile { get; set; }

        [Option('f', "file", Required = true, HelpText = "Event Log file to parse", DefaultValue = "EventLog.evtx")]
        public string EventLogFile { get; set; }

        [Option('z', "timezone", HelpText = "TimeZone for local date times", DefaultValue = null)]
        public string TimeZone { get; set; }
    }
}