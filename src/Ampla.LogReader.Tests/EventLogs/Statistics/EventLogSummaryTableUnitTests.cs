using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Ampla.LogReader.EventLogs.Statistics
{
    [TestFixture]
    public class EventLogSummaryTableUnitTests : TestFixture
    {
        [Test]
        public void Empty()
        {
            EventLogSummaryTable table = new EventLogSummaryTable("Summary");
            DataTable dataTable = table.GetData();

            Assert.That(dataTable, Is.Not.Null);
            Assert.That(dataTable.Rows, Is.Empty);
            Assert.That(dataTable.Columns, Is.Not.Empty);
        }

        [Test]
        public void WithEmptyEventLog()
        {
            EventLogSummaryTable table = new EventLogSummaryTable("Summary");
            IEventLogSystem eventLogSystem = new EventLogSystem();
            EventLogReader reader = (from eventLog in eventLogSystem.GetEventLogs() 
                                     where eventLog.Entries.Count == 0 
                                     select new EventLogReader(eventLog)).FirstOrDefault();

            Assert.That(reader, Is.Not.Null, "No Event Logs found with zero entries");

            table.Add(reader);

            DataTable dataTable = table.GetData();

            Assert.That(dataTable, Is.Not.Null);
            Assert.That(dataTable.Rows.Count, Is.EqualTo(1));
            Assert.That(dataTable.Columns, Is.Not.Empty);
        }
    }
}