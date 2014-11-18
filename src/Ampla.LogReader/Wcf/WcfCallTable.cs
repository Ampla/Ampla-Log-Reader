using System;
using System.Collections.Generic;
using System.Data;

namespace Ampla.LogReader.Wcf
{
    public class WcfCallTable
    {
        private readonly TimeZoneInfo timeZoneInfo;

        public WcfCallTable(TimeZoneInfo timeZoneInfo)
        {
            this.timeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Local;
        }

        private DateTime ConvertToLocalTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
        }

        public DataTable Create(IEnumerable<WcfCall> wcfCalls)
        {
            DataTable dataTable = new DataTable("WcfCalls");
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("CallTimeUtc", typeof(DateTime));
            dataTable.Columns.Add("CallTimeLocal", typeof(DateTime));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Action", typeof(string));
            dataTable.Columns.Add("Method", typeof(string));
            dataTable.Columns.Add("Duration", typeof(double));
            dataTable.Columns.Add("RequestMessage", typeof(string));
            dataTable.Columns.Add("ResponseMessageLength", typeof(double));
            dataTable.Columns.Add("IsFault", typeof(bool));
            dataTable.Columns.Add("FaultCode", typeof(string));
            dataTable.Columns.Add("FaultString", typeof(string));
            dataTable.Columns.Add("FaultDetails", typeof(string));
            dataTable.Columns.Add("Source", typeof(string));

            int count = 0;

            foreach (WcfCall call in wcfCalls)
            {
                dataTable.Rows.Add(++count,
                                   call.CallTime,
                                   ConvertToLocalTime(call.CallTime),
                                   call.Url,
                                   call.Action,
                                   call.Method,
                                   call.Duration.TotalSeconds,
                                   call.RequestMessage,
                                   call.ResponseMessageLength,
                                   call.IsFault,
                                   call.Fault != null ? call.Fault.FaultCode : "",
                                   call.Fault != null ? call.Fault.FaultString : "",
                                   call.Fault != null ? call.Fault.Details : "",
                                   call.Source);
            }

            dataTable.AcceptChanges();
            return dataTable;
        }
    }
}