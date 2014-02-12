using System.IO;

namespace Ampla.LogReader.FileSystem
{
    public class AmplaProject
    {
        public string ProjectName { get; set; }
        public string Directory { get; set; }

        public string WcfLogDirectory
        {
            get
            {
                //%ProgramData%\Citect\Ampla\Projects\{Project}\ReplayLogs\WCFRecorder 
                string folder = Path.Combine(Directory, "ReplayLogs", "WCFRecorder");
                return folder;
            }
        }

        public string RemotingDirectory
        {
            get
            {
                //%ProgramData%\Citect\Ampla\Projects\{Project}\ReplayLogs\PAQueryLoad 
                string folder = Path.Combine(Directory, "ReplayLogs", "PAQueryLoad");
                return folder;
            }
        }
    }
}