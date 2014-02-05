using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.FileSystem
{
    public class WcfLogDirectory : IWcfLogReader
    {
        private readonly string directory;

        private List<WcfCall> wcfCalls = new List<WcfCall>(); 
        
        public WcfLogDirectory(AmplaProject project)
        {
            string folder = project.WcfLogDirectory;
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder + " does not exist.");
            }
            directory = folder;
        }

        public void Read()
        {
            List<WcfCall> calls = new List<WcfCall>();
            IEnumerable<FileInfo> wcfReaderFiles = new DirectoryInfo(directory).EnumerateFiles();
            foreach (FileInfo file in wcfReaderFiles)
            {
                WcfLogReader reader = new WcfLogReader(file.FullName);
                reader.Read();
                calls.AddRange(reader.WcfCalls);
            }
            wcfCalls = calls;
        }

        public List<WcfCall> WcfCalls { get { return wcfCalls; } }
    }
}