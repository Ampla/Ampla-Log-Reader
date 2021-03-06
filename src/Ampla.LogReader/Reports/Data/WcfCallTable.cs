﻿using System;
using System.Collections.Generic;
using System.Data;
using Ampla.LogReader.Wcf;

namespace Ampla.LogReader.Reports.Data
{
    public class WcfCallTable
    {
        private readonly TimeZoneInfo timeZoneInfo;

        public WcfCallTable(IEnumerable<WcfCall> wcfCalls, TimeZoneInfo timeZoneInfo)
        {
            this.timeZoneInfo = timeZoneInfo ?? TimeZoneInfo.Local;
            DataTable data = CreateTable("WcfCalls");
            AddEntries(data, wcfCalls);
            Data = data;
        }

        public DataTable Data { get; private set; }

        private DateTime ConvertToLocalTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
        }

        private DataTable CreateTable(string tableName)
        {

            DataTable dataTable = new DataTable(tableName);
            dataTable.Columns.Add("Id", typeof (int));
            dataTable.Columns.Add("CallTimeUtc", typeof (DateTime));
            dataTable.Columns.Add("CallTimeLocal", typeof (DateTime));
            dataTable.Columns.Add("Url", typeof (string));
            dataTable.Columns.Add("Action", typeof (string));
            dataTable.Columns.Add("Method", typeof (string));
            dataTable.Columns.Add("Duration", typeof (double));
            dataTable.Columns.Add("RequestMessage", typeof (string));
            dataTable.Columns.Add("ResponseMessageLength", typeof (double));
            dataTable.Columns.Add("IsFault", typeof (bool));
            dataTable.Columns.Add("FaultCode", typeof (string));
            dataTable.Columns.Add("FaultString", typeof (string));
            dataTable.Columns.Add("FaultDetails", typeof (string));
            dataTable.Columns.Add("Source", typeof (string));

            return dataTable;
        }

        private void AddEntries(DataTable dataTable, IEnumerable<WcfCall> wcfCalls)
        {
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
        }
    }
}