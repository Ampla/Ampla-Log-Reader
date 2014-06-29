using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Ampla.LogReader.Xml;

namespace Ampla.LogReader.Remoting
{
    public class RemotingEntry
    {
        private RemotingEntry()
        {
        }

        private static readonly char[] Comma = new[] { ',' };
        
        public DateTime CallTime { get; private set; }

        public string Identity { get; private set; }
            
        public string Method { get; private set; }

        public string TypeName { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string ArgumentXml { get; private set; }

        public RemotingArgument[] Arguments { get; private set; } 

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
                ArgumentXml = XmlHelper.GetInnerXml(xmlNode, "__Args"),
                Arguments = RemotingArgument.Parse(XmlHelper.GetNodes(xmlNode, "__Args/*")),
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
            dataTable.Columns.Add("ArgumentCount", typeof(int));
            dataTable.Columns.Add("Argument_01", typeof(string));
            dataTable.Columns.Add("Argument_02", typeof(string));
            dataTable.Columns.Add("Argument_03", typeof(string));
            dataTable.Columns.Add("Argument_04", typeof(string));
            dataTable.Columns.Add("Argument_05", typeof(string));
            dataTable.Columns.Add("Argument_06", typeof(string));
            dataTable.Columns.Add("Argument_07", typeof(string));
            dataTable.Columns.Add("Source", typeof(string));

            int count = 0;

            foreach (RemotingEntry entry in entries)
            {
                string argument01 = entry.Arguments.Length > 0 ? entry.Arguments[0].Value : null;
                string argument02 = entry.Arguments.Length > 1 ? entry.Arguments[1].Value : null;
                string argument03 = entry.Arguments.Length > 2 ? entry.Arguments[2].Value : null;
                string argument04 = entry.Arguments.Length > 3 ? entry.Arguments[3].Value : null;
                string argument05 = entry.Arguments.Length > 4 ? entry.Arguments[4].Value : null;
                string argument06 = entry.Arguments.Length > 5 ? entry.Arguments[5].Value : null;
                string argument07 = entry.Arguments.Length > 6 ? entry.Arguments[6].Value : null;

                dataTable.Rows.Add(++count,
                                   entry.CallTime,
                                   entry.CallTime.ToLocalTime(),
                                   entry.Identity,
                                   entry.TypeName,
                                   entry.Method,
                                   entry.Duration.TotalSeconds,
                                   entry.ArgumentXml,
                                   entry.Arguments.Length,
                                   argument01,
                                   argument02,
                                   argument03,
                                   argument04,
                                   argument05,
                                   argument06,
                                   argument07,
                                   entry.Source);
            }

            dataTable.AcceptChanges();
            return dataTable;

        }
    }
}