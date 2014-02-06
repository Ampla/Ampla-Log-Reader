using CommandLine;

namespace Ampla.LogReader.Console
{
    public class LogReaderOptions
    {
        [Option('p', "project", Required = false, HelpText = "Select the output files to log", DefaultValue = "AmplaProject")]
        public string Project { get; set; }

        [Option('m', "mode", Required = false, HelpText = "Select the Output mode (Text, Xml, Excel)", DefaultValue = OutputMode.Text)]
        public OutputMode OutputMode { get; set; }

        [Option('o', "output", Required = false, HelpText = "Select the Output file", DefaultValue = null)]
        public string OutputFile { get; set; }

        [Option('d', "directory", Required = false, HelpText = "Directory where ReplayLogs is found", DefaultValue = null)]
        public string LogDirectory { get; set; }

        [Option('x', "debug", HelpText = "Attach the debugger", Required = false, DefaultValue = false)]
        public bool Debug { get; set; }
    }
}