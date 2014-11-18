using System;
using System.Linq;

namespace Ampla.LogReader.Statistics
{
    public static class TimeZoneHelper
    {
        public static TimeZoneInfo GetSpecificTimeZone(string timeZone)
        {
            return TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(timeZoneInfo => timeZoneInfo.Id == timeZone);
        }

        public static TimeZoneInfo GetTimeZone()
        {
            return TimeZoneInfo.Local;
        }
    }
}