using System.Collections.Generic;
using Ampla.LogReader.Wcf;
using NUnit.Framework;

namespace Ampla.LogReader.Reports.Data
{
    [TestFixture]
    public class WcfLocationsTableUnitTests : TestFixture
    {
        private const string directory = @".\Wcf\Resources";
        private const int numberOfEntries = 10;

        [Test]
        public void EmptyTable()
        {
            List<WcfCall> list = new List<WcfCall>();
            WcfLocationsTable table = new WcfLocationsTable(list);
            Assert.That(table.Data, Is.Not.Null);
            Assert.That(table.Data.Columns.Count, Is.GreaterThan(5));
            Assert.That(table.Data.Rows.Count, Is.EqualTo(0));
        }

        [Test]
        public void WithData()
        {
            WcfLogDirectory wcfLogDirectory = new WcfLogDirectory(directory);
            Assert.That(wcfLogDirectory.DirectoryExists(), Is.True);
            wcfLogDirectory.Read();

            Assert.That(wcfLogDirectory.Entries, Is.Not.Empty);
            Assert.That(wcfLogDirectory.Entries.Count, Is.EqualTo(numberOfEntries));

            WcfLocationsTable table = new WcfLocationsTable(wcfLogDirectory.Entries);
            Assert.That(table.Data, Is.Not.Null);
            Assert.That(table.Data.Columns.Count, Is.GreaterThan(5));
            Assert.That(table.Data.Rows.Count, Is.GreaterThan(0));
            Assert.That(table.Data.Rows.Count, Is.LessThan(numberOfEntries));

            Assert.That(table.Data.Select("[Operation] = 'GetData'"), Is.Not.Empty, "GetData Rows");
            Assert.That(table.Data.Select("[Operation] = 'GetViews'"), Is.Not.Empty, "GetViews Rows");
            Assert.That(table.Data.Select("[Operation] = 'SubmitData'"), Is.Empty, "SubmitData Rows");
            Assert.That(table.Data.Select("[Operation] = 'Delete'"), Is.Empty, "Delete Rows");
            Assert.That(table.Data.Select("[Operation] = 'Confirm'"), Is.Empty, "Confirm Rows");
        }

        [Test]
        public void SessionGuids()
        {
            WcfLogDirectory logDirectory = new WcfLogDirectory(directory);
            Assert.That(logDirectory.DirectoryExists(), Is.True);
            logDirectory.Read();

            Assert.That(logDirectory.Entries, Is.Not.Empty);
            WcfCall getData = logDirectory.Entries.Find(re => re.Method == "GetData");
            
            Assert.That(getData, Is.Not.Null);
            string user = getData.RequestMessage;

            WcfLocationsTable table = new WcfLocationsTable(logDirectory.Entries);
            Assert.That(table.Data.Select("[Credentials] = 'Session: 8bca9a70-627b-4a14-a2f6-5ec051a94953'"), Is.Not.Empty, "Unable to find user: {0}", user);
        }
    }
}