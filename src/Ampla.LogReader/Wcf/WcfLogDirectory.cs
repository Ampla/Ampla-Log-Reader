using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.FileSystem;

namespace Ampla.LogReader.Wcf
{
    public class WcfLogDirectory : LogReader<WcfCall>
    {
        private readonly string directory;

        public WcfLogDirectory(AmplaProject project) : this(project.WcfLogDirectory)
        {
        }

        public WcfLogDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                //throw new DirectoryNotFoundException(folder + " does not exist.");
            }
            this.directory = directory;
        }

        protected override List<WcfCall> ReadEntries()
        {
            List<WcfCall> list = new List<WcfCall>();
            IEnumerable<FileInfo> wcfReaderFiles = new DirectoryInfo(directory).EnumerateFiles();
            foreach (FileInfo file in wcfReaderFiles)
            {
                WcfLogReader reader = new WcfLogReader(file.FullName);
                reader.Read();
                list.AddRange(reader.Entries);
            }
            return list;
        }

        public bool DirectoryExists()
        {
            return Directory.Exists(directory);
        }

    }
}