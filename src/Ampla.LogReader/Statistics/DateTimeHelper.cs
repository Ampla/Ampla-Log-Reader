using System;

namespace Ampla.LogReader.Statistics
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Truncates the DateTime to the day it occurs on.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime TruncateToLocalDay(this DateTime dateTime)
        {
            DateTime local = dateTime.ToLocalTime();
            TimeSpan timeOffset = local.TimeOfDay;
            return dateTime.Subtract(timeOffset);
        }
    }
}