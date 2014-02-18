using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Ampla.LogReader.EventLogs
{
    public class SimpleEventLogEntry
    {
        public SimpleEventLogEntry(EventLogEntry eventLogEntry)
        {
            Index = eventLogEntry.Index;
            CallTime = eventLogEntry.TimeGenerated.ToUniversalTime();
            Source = eventLogEntry.Source;
            InstanceId = eventLogEntry.InstanceId;
            Category = eventLogEntry.Category;
            EntryType = eventLogEntry.EntryType;
            MachineName = eventLogEntry.MachineName;
            UserName = eventLogEntry.UserName;
            Message = eventLogEntry.Message;
        }

        public int Index { get; private set; }

        public DateTime CallTime { get; private set; }
        
        public string Category { get; private set; }

        public string Source { get; private set; }

        public EventLogEntryType EntryType { get; private set; }

        public long InstanceId { get; private set; }

        public string MachineName { get; private set; }

        public string UserName { get; private set; }

        public string Message { get; private set; }

        public static DataTable CreateDataTable(IEnumerable<SimpleEventLogEntry> entries)
        {
            DataTable dataTable = new DataTable("EventLog");
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Index", typeof(int));
            dataTable.Columns.Add("CallTimeUtc", typeof(DateTime));
            dataTable.Columns.Add("CallTimeLocal", typeof(DateTime));
            dataTable.Columns.Add("Source", typeof(string));
            dataTable.Columns.Add("InstanceId", typeof(long));
            dataTable.Columns.Add("Category", typeof(string));
            dataTable.Columns.Add("EntryType", typeof(string));
            dataTable.Columns.Add("MachineName", typeof(string));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("Message", typeof(string));

            int count = 0;

            foreach (SimpleEventLogEntry entry in entries)
            {
                dataTable.Rows.Add(++count,
                                   entry.Index,
                                   entry.CallTime,
                                   entry.CallTime.ToLocalTime(),
                                   entry.Source,
                                   entry.InstanceId,
                                   entry.Category,
                                   entry.EntryType.ToString(),
                                   entry.MachineName,
                                   entry.UserName,
                                   entry.Message);
            }

            dataTable.AcceptChanges();
            return dataTable;

        }
    }
}