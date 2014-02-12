using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.FileSystem;

namespace Ampla.LogReader.Wcf
{
    public class WcfLogDirectory : ILogReader<WcfCall>
    {
        private readonly string directory;

        private List<WcfCall> entries = new List<WcfCall>(); 
        
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
            List<WcfCall> list = new List<WcfCall>();
            IEnumerable<FileInfo> wcfReaderFiles = new DirectoryInfo(directory).EnumerateFiles();
            foreach (FileInfo file in wcfReaderFiles)
            {
                WcfLogReader reader = new WcfLogReader(file.FullName);
                reader.Read();
                list.AddRange(reader.Entries);
            }
            entries = list;
        }

        public List<WcfCall> Entries { get { return entries; } }
    }
}