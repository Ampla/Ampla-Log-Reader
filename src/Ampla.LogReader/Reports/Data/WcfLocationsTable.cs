using System;
using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Data
{
    public sealed class WcfLocationsTable
    {
        public WcfLocationsTable(IEnumerable<WcfCall> entries)
        {
            DataTable data = CreateTable("Locations");
            AddEntries(data, entries);
            Data = data;
        }

        private static DataTable CreateTable(string name)
        {
            DataTable table = new DataTable(name);
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("CallTimeUtc", typeof(DateTime));
            table.Columns.Add("CallTimeLocal", typeof(DateTime));

            table.Columns.Add("Credentials", typeof(string));
            table.Columns.Add("Module", typeof(string));
            table.Columns.Add("Location", typeof(string));
            table.Columns.Add("Operation", typeof(string));
            table.Columns.Add("MetaData", typeof(string));
            
            table.Columns.Add("IsFault", typeof(bool));
            table.Columns.Add("FaultString", typeof(string));
            
            table.Columns.Add("LocalDate", typeof(DateTime));
            table.Columns.Add("LocalHour", typeof(string));
            table.Columns.Add("DayOfWeek", typeof(string));
            return table;
        }

        public DataTable Data { get; private set; }

        private static void AddEntries(DataTable dataTable, IEnumerable<WcfCall> entries)
        {
            int count = 0;

            foreach (WcfCall call in entries)
            {
                IWcfLocationParameter location = null;
                switch (call.Method)
                {
                    case "GetData":
                        {
                            location = new GetDataParameter(call);
                            break;
                        }
                    case "GetViews":
                        {
                            location = new GetViewsParameter(call);
                            break;
                        }
                    case "Update":
                        {
                            //location = new UpdateParameters(entry);
                            break;
                        }
                    case "GetNewSample":
                        {
                            //location = new GetNewSampleParameters(entry);
                            break;
                        }
                }

                if (location != null)
                {
                    DateTime localTime = call.CallTime.ToLocalTime();

                    dataTable.Rows.Add(
                        ++count,
                        call.CallTime,
                        localTime,

                        location.Credentials,
                        location.Module,
                        location.Location,
                        location.Operation,
                        location.MetaData,
                        
                        call.IsFault,
                        call.Fault != null ? call.Fault.FaultString : "",

                        localTime.Date,
                        TimeSpan.FromHours(localTime.Hour),
                        localTime.DayOfWeek
                        );
                }
            }

            dataTable.AcceptChanges();
        }
    }
}