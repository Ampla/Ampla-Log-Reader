using System.Collections.Generic;
using Ampla.LogReader.Remoting;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Data
{
    [TestFixture]
    public class RemotingEntryTableUnitTests : TestFixture
    {
        private const string directory = @".\Remoting\Resources";
        private const int numberOfEntries = 7;

        [Test]
        public void EmptyTable()
        {
            List<RemotingEntry> list = new List<RemotingEntry>();
            RemotingEntryTable table = new RemotingEntryTable(list);
            Assert.That(table.Data, Is.Not.Null);
            Assert.That(table.Data.Columns.Count, Is.GreaterThan(10));
            Assert.That(table.Data.Rows.Count, Is.EqualTo(0));
        }

        [Test]
        public void WithData()
        {
            RemotingDirectory remotingDirectory = new RemotingDirectory(directory);
            Assert.That(remotingDirectory.DirectoryExists(), Is.True);
            remotingDirectory.Read();

            Assert.That(remotingDirectory.Entries, Is.Not.Empty);
            Assert.That(remotingDirectory.Entries.Count, Is.EqualTo(numberOfEntries));

            RemotingEntryTable table = new RemotingEntryTable(remotingDirectory.Entries);
            Assert.That(table.Data, Is.Not.Null);
            Assert.That(table.Data.Columns.Count, Is.GreaterThan(10));
            Assert.That(table.Data.Rows.Count, Is.EqualTo(numberOfEntries));
        }
    }
}