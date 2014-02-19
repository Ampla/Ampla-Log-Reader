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

        [Test]
        public void WcfOnlyProject()
        {
            AmplaProject project = AmplaTestProjects.GetWcfOnlyProject();
            Assert.That(project, Is.Not.Null);

            Assert.That(Directory.Exists(project.Directory), Is.True, project.Directory);
            Assert.That(Directory.Exists(project.RemotingDirectory), Is.False);
            Assert.That(Directory.Exists(project.WcfLogDirectory), Is.True);
        }

        [Test]
        public void AmplaProject()
        {
            AmplaProject project = AmplaTestProjects.GetAmplaProject();
            Assert.That(project, Is.Not.Null);

            Assert.That(Directory.Exists(project.Directory), Is.True);
            Assert.That(Directory.Exists(project.RemotingDirectory), Is.True);
            Assert.That(Directory.Exists(project.WcfLogDirectory), Is.True);
        }
    }
}