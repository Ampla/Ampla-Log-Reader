using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.FileSystem
{
    [TestFixture]
    public class AmplaProjectUnitTests : TestFixture
    {
        [Test]
        public void WcfLogDirectory()
        {
            AmplaProject project = new AmplaProject
                {
                    ProjectName = "AmplaProject",
                    Directory = Path.Combine(".\\Projects", "AmplaProject")
                };

            Assert.That(project.WcfLogDirectory, Is.StringEnding("ReplayLogs\\WCFRecorder"));
        }

        [Test]
        public void RemotingDirectory()
        {
            AmplaProject project = new AmplaProject
            {
                ProjectName = "AmplaProject",
                Directory = Path.Combine(".\\Projects", "AmplaProject")
            };

            Assert.That(project.RemotingDirectory, Is.StringEnding("ReplayLogs\\PAQueryLoad"));
        }

    }
}