using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.FileSystem;

namespace Ampla.LogReader.Wcf
{
    public class WcfLogDirectory : LogReader<WcfCall>
    {
        private readonly string directory;

        public WcfLogDirectory(AmplaProject project)
        {
            string folder = project.WcfLogDirectory;
            if (!Directory.Exists(folder))
            {
                //throw new DirectoryNotFoundException(folder + " does not exist.");
            }
            directory = folder;
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

    }
}