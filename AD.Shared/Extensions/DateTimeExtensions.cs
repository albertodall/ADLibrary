using System;

namespace AD.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The DateTime structure's AddDays method increments (or decrements) a date and time by a specified number of days, 
        /// but what if you need to adjust a date by a specified number of weekdays? Use the AddWeekdays extension method 
        /// to add (or subtract) a specified number of weekdays.
        /// </summary>
        public static DateTime AddWeekdays(this DateTime date, int days)
        {
            var sign = days < 0 ? -1 : 1;
            var unsignedDays = Math.Abs(days);
            var weekdaysAdded = 0;
            while (weekdaysAdded < unsignedDays)
            {
                date = date.AddDays(sign);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    weekdaysAdded++;
            }
            return date;
        }

        /// <summary>
        /// The DateTime structure offers methods to add time units, but there is no method to set an existing DateTime variable 
        /// to a particular time, such as 5:00 PM. The SetTime extension method provides this functionality, allowing a developer 
        /// to assign a DateTime variable's hour, minute, second, and/or millisecond settings.
        /// </summary>
        public static DateTime SetTime(this DateTime date, int hour)
        {
            return date.SetTime(hour, 0, 0, 0);
        }
        
        public static DateTime SetTime(this DateTime date, int hour, int minute)
        {
            return date.SetTime(hour, minute, 0, 0);
        }
        
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
        {
            return date.SetTime(hour, minute, second, 0);
        }
        
        public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// Returns the 1st of the specified date's month/year
        /// </summary>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Uses the DateTime structure's DaysInMonth method to determine the last day of the specified date's month/year.
        /// </summary>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        /// <summary>
        /// Generates a relative date string for a date compared to the current date and time.
        /// </summary>
        public static string ToRelativeDateString(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.Now);
        }

        /// <summary>
        /// Generates a relative date string for a date compared to the current UTC time.
        /// </summary>
        public static string ToRelativeDateStringUtc(this DateTime date)
        {
            return GetRelativeDateValue(date, DateTime.UtcNow);
        }

        private static string GetRelativeDateValue(DateTime date, DateTime comparedTo)
        {
            TimeSpan diff = comparedTo.Subtract(date);
            if (diff.TotalDays >= 365)
                return string.Concat("on ", date.ToString("MMMM d, yyyy"));
            if (diff.TotalDays >= 7)
                return string.Concat("on ", date.ToString("MMMM d"));
            if (diff.TotalDays > 1)
                return string.Format("{0:N0} days ago", diff.TotalDays);
            if (diff.TotalDays == 1)
                return "yesterday";
            if (diff.TotalHours >= 2)
                return string.Format("{0:N0} hours ago", diff.TotalHours);
            if (diff.TotalMinutes >= 60)
                return "more than an hour ago";
            if (diff.TotalMinutes >= 5)
                return string.Format("{0:N0} minutes ago", diff.TotalMinutes);
            return diff.TotalMinutes >= 1 ? "a few minutes ago" : "less than a minute ago";
        }

        /// <summary>
        /// Converts a UTC time to a time in the given time zone
        /// </summary>
        /// <param name="targetTimeZone">The time zone to convert to</param>
        /// <param name="utcTime">The UTC time</param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this DateTime utcTime, TimeZoneInfo targetTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, targetTimeZone);
        }

        /// <summary>
        /// Converts a local time to a UTC time
        /// </summary>
        /// <param name="sourceTimeZone">The time zone of the local time</param>
        /// <param name="localTime">The local time</param>
        /// <returns></returns>
        public static DateTime ToUniversalTime(this DateTime localTime, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime, sourceTimeZone);
        }
    }
}
