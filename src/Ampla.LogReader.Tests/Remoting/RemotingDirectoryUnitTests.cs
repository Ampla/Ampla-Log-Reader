using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.Remoting
{
    [TestFixture]
    public class RemotingDirectoryUnitTests : TestFixture
    {
        private const string directory = @".\Remoting\Resources";

        private const int numFiles = 9;

        [Test]
        public void InvalidDirectory()
        {
            string invalid = Path.Combine(directory, "Invalid");
            RemotingDirectory remotingDirectory = new RemotingDirectory(invalid);
            Assert.That(remotingDirectory.DirectoryExists(), Is.False);
        }

        [Test]
        public void WithFiles()
        {
            RemotingDirectory remotingDirectory = new RemotingDirectory(directory);
            Assert.That(remotingDirectory.DirectoryExists(), Is.True);
            remotingDirectory.Read();

            Assert.That(remotingDirectory.Entries, Is.Not.Empty);
            Assert.That(remotingDirectory.Entries.Count, Is.EqualTo(numFiles));
        }

    }

}