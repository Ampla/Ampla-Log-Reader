using System;
using System.Collections.Generic;
using System.IO;

namespace Ampla.LogReader.Wcf
{
    public class WcfLogDirectory : IWcfLogReader
    {
        private readonly string directory;

        private List<WcfCall> wcfCalls = new List<WcfCall>(); 
        
        public WcfLogDirectory(string project)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder + " does not exist.");
            }

            folder = Path.Combine(folder, "Citect", "Ampla", "Projects");
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder + " does not exist.");
            }

            //%ProgramData%\Citect\Ampla\Projects\{Project}\ReplayLogs\WCFRecorder 
            folder = Path.Combine(folder, project, "ReplayLogs", "WCFRecorder");

            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder + " does not exist.");
            }
            directory = folder;
        }

        public void Execute()
        {
            List<WcfCall> calls = new List<WcfCall>();
            IEnumerable<FileInfo> wcfReaderFiles = new DirectoryInfo(directory).EnumerateFiles();
            foreach (FileInfo file in wcfReaderFiles)
            {
                WcfLogReader reader = new WcfLogReader(file.FullName);
                reader.Execute();
                calls.AddRange(reader.WcfCalls);
            }
            wcfCalls = calls;
        }

        public List<WcfCall> WcfCalls { get { return wcfCalls; } }
    }
}