using System.Collections.Generic;
using System.IO;
using Ampla.LogReader.FileSystem;

namespace Ampla.LogReader.Remoting
{
    public class RemotingDirectory : LogReader<RemotingEntry>
    {
        private readonly string directory;

        public RemotingDirectory(string directory)
        {
            this.directory = directory;
            Name = DirectoryExists() ? new DirectoryInfo(this.directory).Name : directory;
        }

        public RemotingDirectory(AmplaProject project) : this(project.RemotingDirectory)
        {
        }

        protected override List<RemotingEntry> ReadEntries()
        {
            List<RemotingEntry> list = new List<RemotingEntry>();

            if (DirectoryExists())
            {
                IEnumerable<FileInfo> remotingFiles = new DirectoryInfo(directory).EnumerateFiles();
                foreach (FileInfo file in remotingFiles)
                {
                    RemotingLogReader reader = new RemotingLogReader(file.FullName);
                    reader.ReadAll();
                    list.AddRange(reader.Entries);
                }
            }
            return list;
        }

        public bool DirectoryExists()
        {
            return Directory.Exists(directory);
        }
    }
}