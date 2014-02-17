using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingEntry
    {
        private static readonly char[] Comma = new[] { ',' };
        
        public DateTime CallTime { get; private set; }

        public string Identity { get; private set; }
            
        public string Method { get; private set; }

        public string TypeName { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string Arguments { get; private set; }

        public string Source { get; set; }

        public static RemotingEntry LoadFromXml(XmlNode xmlNode)
        {
            RemotingEntry entry = new RemotingEntry
            {
                CallTime = XmlHelper.GetDateTime(xmlNode, "UTCDateTime", DateTime.MinValue),
                Identity = XmlHelper.GetValue(xmlNode, "Identity", string.Empty),
                Method = XmlHelper.GetValue(xmlNode, "__MethodName", string.Empty),
                TypeName = XmlHelper.GetValue(xmlNode, "__TypeName", string.Empty),
                Duration = TimeSpan.FromMilliseconds(XmlHelper.GetValue(xmlNode, "__MessageResponseTime", 0D)),
                Arguments = XmlHelper.GetInnerXml(xmlNode, "__Args"),
            };

            if (!string.IsNullOrEmpty(entry.TypeName))
            {
                entry.TypeName = entry.TypeName.Split(Comma, StringSplitOptions.None)[0];
            }
            
            return entry;
        }

        public static DataTable CreateDataTable(IEnumerable<RemotingEntry> entries)
        {
            DataTable dataTable = new DataTable("RemotingEntries");
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("CallTimeUtc", typeof(DateTime));
            dataTable.Columns.Add("CallTimeLocal", typeof(DateTime));
            dataTable.Columns.Add("Identity", typeof(string));
            dataTable.Columns.Add("TypeName", typeof(string));
            dataTable.Columns.Add("Method", typeof(string));
            dataTable.Columns.Add("Duration", typeof(double));
            dataTable.Columns.Add("Arguments", typeof(string));
            dataTable.Columns.Add("Source", typeof(string));

            int count = 0;

            foreach (RemotingEntry entry in entries)
            {
                dataTable.Rows.Add(++count,
                                   entry.CallTime,
                                   entry.CallTime.ToLocalTime(),
                                   entry.Identity,
                                   entry.TypeName,
                                   entry.Method,
                                   entry.Duration.TotalSeconds,
                                   entry.Arguments,
                                   entry.Source);
            }

            dataTable.AcceptChanges();
            return dataTable;

        }
    }
}