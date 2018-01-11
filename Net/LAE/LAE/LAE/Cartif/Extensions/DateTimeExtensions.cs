using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Provides additional methods for working with DateTime objects. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class DateTimeExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the number of milliseconds ellapsed since the start of UNIX time which started
        ///           January 1st, 1970. This is the same baseline as Javascript's getTime() method making
        ///           this useful for calculating timings. </summary>
        /// <value> The milliseconds since unix time. </value>
        ///--------------------------------------------------------------------------------------------------
        public static double MillisecondsSinceUNIXTime
        {
            get
            {
                return (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets a datetime for tomorrow. </summary>
        /// <value> The tomorrow. </value>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime Tomorrow
        {
            get
            {
                return DateTime.Today.AddDays(1);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets a datetime for yesterday. </summary>
        /// <value> The yesterday. </value>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime Yesterday
        {
            get
            {
                return DateTime.Today.AddDays(-1);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a datetime in the past, set at today minus the timespan. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime Ago(this TimeSpan value)
        {
            return DateTime.Now.Subtract(value);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a datetime in the future, set at today plus the timespan. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime FromNow(this TimeSpan value)
        {
            return DateTime.Now.Add(value);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Monday is 1. 2x faster than default dayOfWeekMethod. source:
        ///           http://stackoverflow.com/questions/22258070/datetime-dayofweek-micro-
        ///           optimization/22278311#22278311. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="date"> The date to act on. </param>
        /// <returns> The DayOfWeek. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int DayOfWeekTurbo(this DateTime date)
        {
            int dow = (int)(((date.Ticks >> 14) / 52734375L) + 1) % 7;
            return dow == 0 ? 7 : dow;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a new DateTime with the day set as 1 without instantiation source:
        ///           http://stackoverflow.com/questions/5002556/set-datetime-to-start-of-month. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="date"> The date to act on. </param>
        /// <returns> The DayOfWeek. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime ToFirstDayOfMonth(this DateTime date)
        {
            return date.AddDays(1 - date.Day);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a new DateTime with the day set as 1 without instantiation source:
        ///           http://stackoverflow.com/questions/5002556/set-datetime-to-start-of-month. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="date"> The date to act on. </param>
        /// <returns> The DayOfWeek. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime ToLastDayOfMonth(this DateTime date)
        {
            return date.AddMonths(1).AddDays(-date.Day);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns the number of weekdays between two dates. Calls DateTimeUtils.WeekDays. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="dtmStart"> The dtmStart to act on. </param>
        /// <param name="dtmEnd">   The dtm end Date/Time. </param>
        /// <returns> The DayOfWeek. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int WeekDays(this DateTime dtmStart, DateTime dtmEnd)
        {
            return DateTimeUtils.WeekDays(dtmStart, dtmEnd);
        }

        #region Elapsed extension

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Elapseds the time. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime"> The datetime. </param>
        /// <returns> TimeSpan. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Elapsed(this DateTime datetime)
        {
            return DateTime.Now - datetime;
        }
        #endregion

        #region Week of year

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Weeks the of year. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime">       The datetime. </param>
        /// <param name="weekrule">       The weekrule. </param>
        /// <param name="firstDayOfWeek"> The first day of week. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule, DayOfWeek firstDayOfWeek)
        {
            System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Weeks the of year. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime">       The datetime. </param>
        /// <param name="firstDayOfWeek"> The first day of week. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.CalendarWeekRule weekrule = dateinf.CalendarWeekRule;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Weeks the of year. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime"> The datetime. </param>
        /// <param name="weekrule"> The weekrule. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekrule)
        {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Weeks the of year. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime"> The datetime. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int WeekOfYear(this DateTime datetime)
        {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.CalendarWeekRule weekrule = dateinf.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return WeekOfYear(datetime, weekrule, firstDayOfWeek);
        }
        #endregion

        #region Get Datetime for Day of Week

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the date time for day of week. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime">       The datetime. </param>
        /// <param name="day">            The day. </param>
        /// <param name="firstDayOfWeek"> The first day of week. </param>
        /// <returns> The date time for day of week. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day, DayOfWeek firstDayOfWeek)
        {
            int current = DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek);
            int resultday = DaysFromFirstDayOfWeek(day, firstDayOfWeek);
            return datetime.AddDays(resultday - current);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DateTime extension method that gets date time for day of week. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime"> The datetime. </param>
        /// <param name="day">      The day. </param>
        /// <returns> The date time for day of week. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day)
        {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return GetDateTimeForDayOfWeek(datetime, day, firstDayOfWeek);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Firsts the date time of week. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime"> The datetime. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime)
        {
            System.Globalization.DateTimeFormatInfo dateinf = new System.Globalization.DateTimeFormatInfo();
            DayOfWeek firstDayOfWeek = dateinf.FirstDayOfWeek;
            return FirstDateTimeOfWeek(datetime, firstDayOfWeek);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Firsts the date time of week. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime">       The datetime. </param>
        /// <param name="firstDayOfWeek"> The first day of week. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime FirstDateTimeOfWeek(this DateTime datetime, DayOfWeek firstDayOfWeek)
        {
            return datetime.AddDays(-DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Dayses from first day of week. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="current">        The current. </param>
        /// <param name="firstDayOfWeek"> The first day of week. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        private static int DaysFromFirstDayOfWeek(DayOfWeek current, DayOfWeek firstDayOfWeek)
        {
            //Sunday = 0,Monday = 1,...,Saturday = 6
            int daysbetween = current - firstDayOfWeek;
            if (daysbetween < 0) daysbetween = 7 + daysbetween;
            return daysbetween;
        }
        #endregion

        #region ValueOrDefault

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DateTime? extension method that gets value or default to string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime">     The datetime. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> The value or default to string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string GetValueOrDefaultToString(this DateTime? datetime, string defaultvalue)
        {
            if (datetime == null) return defaultvalue;
            return datetime.Value.ToString();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DateTime? extension method that gets value or default to string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="datetime">     The datetime. </param>
        /// <param name="format">       Describes the format to use. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> The value or default to string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string GetValueOrDefaultToString(this DateTime? datetime, string format, string defaultvalue)
        {
            if (datetime == null) return defaultvalue;
            return datetime.Value.ToString(format);
        }

        #endregion
    }
}
