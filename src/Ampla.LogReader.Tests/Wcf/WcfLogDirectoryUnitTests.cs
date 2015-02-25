using System;
using System.IO;
using NUnit.Framework;

namespace Ampla.LogReader.Wcf
{
    [TestFixture]
    public class WcfLogDirectoryUnitTests : TestFixture
    {
        private const string directory = @".\Wcf\Resources";

        private const int numFiles = 11;

        [Test]
        public void InvalidDirectory()
        {
            string invalid = Path.Combine(directory, "Invalid");
            WcfLogDirectory wcfLogDirectory = new WcfLogDirectory(invalid);
            Assert.That(wcfLogDirectory.DirectoryExists(), Is.False);
            Assert.That(wcfLogDirectory.Name, Is.EqualTo(invalid));
        }

        [Test]
        public void WithFiles()
        {
            WcfLogDirectory wcfLogDirectory = new WcfLogDirectory(directory);
            Assert.That(wcfLogDirectory.DirectoryExists(), Is.True);
            wcfLogDirectory.ReadAll();

            Assert.That(wcfLogDirectory.Entries, Is.Not.Empty);
            Assert.That(wcfLogDirectory.Entries.Count, Is.EqualTo(numFiles));
            Assert.That(wcfLogDirectory.Name, Is.EqualTo("Resources"));
        }

    }
}