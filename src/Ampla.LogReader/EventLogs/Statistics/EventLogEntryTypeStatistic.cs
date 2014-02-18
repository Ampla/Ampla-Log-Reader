using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ampla.LogReader.Statistics;

namespace Ampla.LogReader.EventLogs.Statistics
{
    public class EventLogEntryTypeStatistic : Statistic<SimpleEventLogEntry>
    {
        private readonly List<int> eventTypes = new List<int>();

        public EventLogEntryTypeStatistic(string name) : base(name)
        {
        }

        public override void Add(SimpleEventLogEntry entry)
        {
            switch (entry.EntryType)
            {
                case EventLogEntryType.Error:
                    {
                        ErrorCount++;
                        break;
                    }
                case EventLogEntryType.Warning:
                    {
                        WarningCount++;
                        break;
                    }
                case EventLogEntryType.Information:
                    {
                        InformationCount++;
                        break;
                    }
                case EventLogEntryType.SuccessAudit:
                    {
                        SuccessAuditCount++;
                        break;
                    }
                case EventLogEntryType.FailureAudit:
                    {
                        FailureAuditCount++;
                        break;
                    }
                default:
                    {
                        int eventType = (int)entry.EntryType;
                        if (!eventTypes.Contains(eventType))
                        {
                            eventTypes.Add(eventType);
                        }
                        OtherCount++;
                        break;
                    }
            }
        }

        public override IEnumerable<Result> Results
        {
            get {
                yield return Result.New(Name, "Error Count", ErrorCount);
                yield return Result.New(Name, "Warning Count", WarningCount);
                yield return Result.New(Name, "Information Count", InformationCount);
                yield return Result.New(Name, "Success Audit Count", SuccessAuditCount);
                yield return Result.New(Name, "Failure Audit Count", FailureAuditCount);
                if (OtherCount > 0)
                {
                    string name = string.Format("Other ({0}) Count", string.Join(", ", eventTypes));
                    yield return Result.New(Name, name, OtherCount);
                }
            }
        }

        public int ErrorCount { get; private set; }
        public int WarningCount { get; private set; }
        public int InformationCount { get; private set; }
        public int SuccessAuditCount { get; private set; }
        public int FailureAuditCount { get; private set; }
        public int OtherCount { get; private set; }

        /// <summary>
        /// Compares the EventLogEntry Statistics
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private static int CompareByCountDesc(EventLogEntryTypeStatistic x, EventLogEntryTypeStatistic y)
        {
            int compare = x.ErrorCount.CompareTo(y.ErrorCount);
            if (compare == 0)
            {
                compare = x.WarningCount.CompareTo(y.WarningCount);
            }
            if (compare == 0)
            {
                compare = x.InformationCount.CompareTo(y.InformationCount);
            }
            if (compare == 0)
            {
                compare = x.FailureAuditCount.CompareTo(y.FailureAuditCount);
            }
            if (compare == 0)
            {
                compare = x.SuccessAuditCount.CompareTo(y.SuccessAuditCount);
            }
            if (compare == 0)
            {
                compare = x.OtherCount.CompareTo(y.OtherCount);
            }
            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(x.Name, y.Name);
            }
            return -compare;
        }

        public static IComparer<EventLogEntryTypeStatistic> CompareByCountDesc()
        {
            return new LogReader.Statistics.Comparer<EventLogEntryTypeStatistic>(CompareByCountDesc);
        }


    }
}