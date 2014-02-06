using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader
{
    public class TempDirectory
    {
        private readonly string directoryName;
        private readonly string pattern;
        private int count;

        public TempDirectory(string directoryName, string pattern)
        {
            this.directoryName = directoryName;
            this.pattern = pattern;
        }

        public void DeleteAllFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(directoryName);
            if (directory.Exists)
            {
                foreach (FileInfo file in directory.EnumerateFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                directory.Create();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetNextTemporaryFile()
        {
            string name = string.Format(pattern, ++count);
            FileInfo file = new FileInfo(Path.Combine(directoryName, name));
            if (file.Exists)
            {
                file.Delete();
            }
            Assert.That(file.Exists, Is.False);
            return file.FullName;
        }
    }
}