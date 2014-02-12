using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.FileSystem;

namespace Ampla.LogReader.Remoting
{
    public class RemotingDirectory : ILogReader<RemotingEntry>
    {
        private readonly string directory;
        private List<RemotingEntry> entries;

        public RemotingDirectory(AmplaProject project)
        {
            string folder = project.RemotingDirectory;
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException(folder + " does not exist.");
            }
            directory = folder;
        }

        public void Read()
        {
            List<RemotingEntry> list = new List<RemotingEntry>();
            IEnumerable<FileInfo> remotingFiles = new DirectoryInfo(directory).EnumerateFiles();
            foreach (FileInfo file in remotingFiles)
            {
                RemotingLogReader reader = new RemotingLogReader(file.FullName);
                reader.Read();
                list.AddRange(reader.Entries);
            }
            entries = list;
        }

        public List<RemotingEntry> Entries { get { return entries; } }
    }
}