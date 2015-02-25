using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace Ampla.LogReader.EventLogs
{
    public class SimpleEventLogEntry
    {
        public SimpleEventLogEntry(EventRecord eventRecord)
        {
            Index = (int)GetValue(eventRecord.RecordId, 0);
            CallTime = GetValue(eventRecord.TimeCreated, DateTime.MinValue).ToUniversalTime();
            Source = eventRecord.ProviderName;
            InstanceId = GetValue(eventRecord.Id, 0);
            Category = GetCategory(eventRecord);
            EntryType = GetEntryType(eventRecord);
            MachineName = eventRecord.MachineName;
            UserName = eventRecord.UserId != null ? eventRecord.UserId.Value : "";
            Message = GetMessage(eventRecord);
        }

        private T GetValue<T>(T? nullable, T defaultValue) where T : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValue;
        }

        private string GetCategory(EventRecord eventRecord)
        {
            try
            {
                return eventRecord.LevelDisplayName;
            }
            catch (EventLogNotFoundException)
            {
                return Convert.ToString(eventRecord.Level);
            }
        }

        private string GetMessage(EventRecord eventRecord)
        {
            string message = eventRecord.FormatDescription();
            if (string.IsNullOrEmpty(message))
            {
                if (eventRecord.Properties.Count > 1)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat("{0} properties", eventRecord.Properties.Count);
                    foreach (EventProperty property in eventRecord.Properties)
                    {
                        builder.AppendLine();
                        builder.AppendFormat("{0}", property.Value);
                    }
                    message = builder.ToString();
                }
                else
                {
                    message = eventRecord.Properties.Select(property => Convert.ToString(property.Value)).FirstOrDefault();
                }
            }

            return message;
        }

        private EventLogEntryType GetEntryType(EventRecord eventRecord)
        {
            byte? level = eventRecord.Level;
            if (level.HasValue)
            {
                switch (level.Value)
                {
                    case 3:
                        {
                            return EventLogEntryType.Warning;
                        }
                    case 2: 
                        {
                            return  EventLogEntryType.Error;
                        }
                }
            }

            return EventLogEntryType.Information;
        }

        public SimpleEventLogEntry(EventLogEntry eventLogEntry)
        {
            Index = eventLogEntry.Index;
            CallTime = eventLogEntry.TimeGenerated.ToUniversalTime();
            Source = eventLogEntry.Source;
            InstanceId = eventLogEntry.InstanceId;
            Category = Ignore(eventLogEntry.Category, "(0)");
            EntryType = eventLogEntry.EntryType;
            MachineName = eventLogEntry.MachineName;
            UserName = eventLogEntry.UserName ?? "";
            Message = eventLogEntry.Message;
        }

        private string Ignore(string value, string ignoreIfValue)
        {
            return StringComparer.InvariantCulture.Compare(value, ignoreIfValue) == 0 ? null : value;
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("Index: {0}\r\n", Index);
            builder.AppendFormat("Call Time: {0}\r\n", CallTime);
            builder.AppendFormat("Source: {0}\r\n", Source);
            builder.AppendFormat("InstanceId: {0}\r\n", InstanceId);
            builder.AppendFormat("Category: {0}\r\n", Category);
            builder.AppendFormat("EntryType: {0}\r\n", EntryType);
            builder.AppendFormat("MachineName: {0}\r\n", MachineName);
            builder.AppendFormat("UserName: {0}\r\n", UserName);
            builder.AppendFormat("Message:\r\n {0}", Message);

            return builder.ToString();
        }
    }
}