using System.Collections.Generic;
using Ampla.LogReader.Remoting;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Data
{
    [TestFixture]
    public class RemotingLocationsTableUnitTests : TestFixture
    {
        private const string directory = @".\Remoting\Resources";
        private const int numberOfEntries = 7;

        [Test]
        public void EmptyTable()
        {
            List<RemotingEntry> list = new List<RemotingEntry>();
            RemotingLocationsTable table = new RemotingLocationsTable(list);
            Assert.That(table.Data, Is.Not.Null);
            Assert.That(table.Data.Columns.Count, Is.GreaterThan(5));
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

            RemotingLocationsTable table = new RemotingLocationsTable(remotingDirectory.Entries);
            Assert.That(table.Data, Is.Not.Null);
            Assert.That(table.Data.Columns.Count, Is.GreaterThan(5));
            Assert.That(table.Data.Rows.Count, Is.GreaterThan(0));
            Assert.That(table.Data.Rows.Count, Is.LessThan(numberOfEntries));

            Assert.That(table.Data.Select("[Operation] = 'Query'"), Is.Not.Empty, "Query Rows");
            Assert.That(table.Data.Select("[Operation] = 'Update'"), Is.Not.Empty, "Update Rows");
            Assert.That(table.Data.Select("[Operation] = 'New Record'"), Is.Not.Empty, "New Record Rows");
            Assert.That(table.Data.Select("[Operation] = 'Confirm'"), Is.Empty, "Confirm Rows");
        }
    }
}