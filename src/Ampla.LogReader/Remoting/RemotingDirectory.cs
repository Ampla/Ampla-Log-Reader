using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.FileSystem;

namespace Ampla.LogReader.Remoting
{
    public class RemotingDirectory : LogReader<RemotingEntry>
    {
        private readonly string directory;

        public RemotingDirectory(AmplaProject project)
        {
            string folder = project.RemotingDirectory;
            if (!Directory.Exists(folder))
            {
                //throw new DirectoryNotFoundException(folder + " does not exist.");
            }
            directory = folder;
        }

        protected override List<RemotingEntry> ReadEntries()
        {
            List<RemotingEntry> list = new List<RemotingEntry>();
            IEnumerable<FileInfo> remotingFiles = new DirectoryInfo(directory).EnumerateFiles();
            foreach (FileInfo file in remotingFiles)
            {
                RemotingLogReader reader = new RemotingLogReader(file.FullName);
                reader.Read();
                list.AddRange(reader.Entries);
            }
            return list;
        }

    }
}