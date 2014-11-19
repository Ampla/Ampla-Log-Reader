using System;
using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.Remoting;

namespace Ampla.LogReader.Reports.Data
{
    public sealed class RemotingEntryTable 
    {
        public RemotingEntryTable(IEnumerable<RemotingEntry> entries)
        {
            DataTable data = CreateTable("RemotingEntries");
            AddEntries(data, entries);
            Data = data;
        }

        private DataTable CreateTable(string name)
        {
            DataTable table = new DataTable(name);
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("CallTimeUtc", typeof(DateTime));
            table.Columns.Add("CallTimeLocal", typeof(DateTime));
            table.Columns.Add("Identity", typeof(string));
            table.Columns.Add("TypeName", typeof(string));
            table.Columns.Add("Method", typeof(string));
            table.Columns.Add("Duration", typeof(double));
            table.Columns.Add("Arguments", typeof(string));
            table.Columns.Add("ArgumentCount", typeof(int));
            table.Columns.Add("Argument_01", typeof(string));
            table.Columns.Add("Argument_02", typeof(string));
            table.Columns.Add("Argument_03", typeof(string));
            table.Columns.Add("Argument_04", typeof(string));
            table.Columns.Add("Argument_05", typeof(string));
            table.Columns.Add("Argument_06", typeof(string));
            table.Columns.Add("Argument_07", typeof(string));
            return table;
        }

        public DataTable Data { get; private set; }


        private void AddEntries(DataTable dataTable, IEnumerable<RemotingEntry> entries)
        {
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
                                   entry.CallTimeUtc,
                                   entry.CallTimeLocal,
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
                                   argument07);
            }

            dataTable.AcceptChanges();
        }
    }
}