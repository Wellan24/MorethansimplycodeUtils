using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Provides additional methods for working with integer (or long) values. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class IntegerExtensions
    {
        public const decimal DaysInAMonth = 30.4368499m;    /* The days in a month */
        public const decimal DaysInAYear = 365.242199m; /* The days in a year */

        #region Integer shorthands

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of seconds. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A time span with the specified number of seconds. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Seconds(this int value)
        {
            return new TimeSpan(0, 0, value);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of minutes. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A time span with the specified number of minutes. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Minutes(this int value)
        {
            return new TimeSpan(0, value, 0);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of hours. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A time span with the specified number of hours. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Hours(this int value)
        {
            return new TimeSpan(value, 0, 0);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of days. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A time span with the specified number of days. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Days(this int value)
        {
            return new TimeSpan(value, 0, 0, 0);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of months. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A time span with the specified number of monts. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Months(this int value)
        {
            return value.Months(false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of months. The average number of days in a
        ///           month is used, if you want to assume 30 days per month, call the other method. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <exception name="OverflowException"> Thrown when an arithmetic overflow occurs. </exception>
        /// <param name="value">                The value. </param>
        /// <param name="assume30DaysPerMonth"> Flag indicating whether to assume that a month has 30 days or
        ///                                     to use the average number of days per month to calculate the
        ///                                     time span. </param>
        /// <returns> A time span with the specified number of monts. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Months(this int value, bool assume30DaysPerMonth)
        {
            try
            {
                decimal daysPerMonth = assume30DaysPerMonth ? 30 : DaysInAMonth;

                return new TimeSpan((int)Math.Round(value * daysPerMonth), 0, 0, 0);
            }
            catch (OverflowException ex)
            {
                const int MaximumNumberOfMonths = (int)(int.MaxValue / DaysInAMonth);
                throw new OverflowException("The maximum number of months is " + MaximumNumberOfMonths, ex);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of years. The average number of days in a
        ///           year is used, if you want to assume 365 days per year, call the other method. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> A time span with the specified number of years. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Years(this int value)
        {
            return value.Years(false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a time span with the specified number of years. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <exception name="OverflowException"> Thrown when an arithmetic overflow occurs. </exception>
        /// <param name="value">                The value. </param>
        /// <param name="assume365DaysPerYear"> Flag indicating whether to assume that a year has 365 days or
        ///                                     to use the average number of days per year to calculate the
        ///                                     time span. </param>
        /// <returns> A time span with the specified number of years. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TimeSpan Years(this int value, bool assume365DaysPerYear)
        {
            try
            {
                decimal daysPerYear = assume365DaysPerYear ? 365 : DaysInAYear;

                return new TimeSpan((int)Math.Round(value * daysPerYear), 0, 0, 0);
            }
            catch (OverflowException ex)
            {
                const int MaximumNumberOfYears = (int)(int.MaxValue / DaysInAYear);
                throw new OverflowException("The maximum number of years is " + MaximumNumberOfYears, ex);
            }
        }

        #endregion

        #region Int Date

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that seconds ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="seconds"> The seconds to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime SecondsAgo(this int seconds)
        {
            return seconds.SecondsAgo(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that seconds ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="seconds"> The seconds to act on. </param>
        /// <param name="now">     The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime SecondsAgo(this int seconds, DateTime now)
        {
            return now.AddSeconds(-seconds);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that seconds from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="seconds"> The seconds to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime SecondsFromNow(this int seconds)
        {
            return seconds.SecondsFromNow(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that seconds from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="seconds"> The seconds to act on. </param>
        /// <param name="now">     The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime SecondsFromNow(this int seconds, DateTime now)
        {
            return now.AddSeconds(seconds);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that minutes ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="minutes"> The minutes to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MinutesAgo(this int minutes)
        {
            return minutes.MinutesAgo(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that minutes ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="minutes"> The minutes to act on. </param>
        /// <param name="now">     The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MinutesAgo(this int minutes, DateTime now)
        {
            return now.AddMinutes(-minutes);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that minutes from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="minutes"> The minutes to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MinutesFromNow(this int minutes)
        {
            return minutes.MinutesFromNow(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that minutes from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="minutes"> The minutes to act on. </param>
        /// <param name="now">     The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MinutesFromNow(this int minutes, DateTime now)
        {
            return now.AddMinutes(minutes);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that hours ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="hours"> The hours to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime HoursAgo(this int hours)
        {
            return hours.HoursAgo(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that hours ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="hours"> The hours to act on. </param>
        /// <param name="now">   The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime HoursAgo(this int hours, DateTime now)
        {
            return now.AddHours(-hours);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that hours from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="hours"> The hours to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime HoursFromNow(this int hours)
        {
            return hours.HoursFromNow(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that hours from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="hours"> The hours to act on. </param>
        /// <param name="now">   The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime HoursFromNow(this int hours, DateTime now)
        {
            return now.AddHours(hours);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that days ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="days"> The days to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime DaysAgo(this int days)
        {
            return days.DaysAgo(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that days ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="days"> The days to act on. </param>
        /// <param name="now">  The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime DaysAgo(this int days, DateTime now)
        {
            return now.AddDays(-days);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that days from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="days"> The days to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime DaysFromNow(this int days)
        {
            return days.DaysFromNow(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that days from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="days"> The days to act on. </param>
        /// <param name="now">  The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime DaysFromNow(this int days, DateTime now)
        {
            return now.AddDays(days);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that months ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="months"> The months to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MonthsAgo(this int months)
        {
            return months.MonthsAgo(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that months ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="months"> The months to act on. </param>
        /// <param name="now">    The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MonthsAgo(this int months, DateTime now)
        {
            return now.AddMonths(-months);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that months from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="months"> The months to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MonthsFromNow(this int months)
        {
            return months.MonthsFromNow(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that months from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="months"> The months to act on. </param>
        /// <param name="now">    The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime MonthsFromNow(this int months, DateTime now)
        {
            return now.AddMonths(months);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that years ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="years"> The years to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime YearsAgo(this int years)
        {
            return years.YearsAgo(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that years ago. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="years"> The years to act on. </param>
        /// <param name="now">   The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime YearsAgo(this int years, DateTime now)
        {
            return now.AddYears(-years);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that years from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="years"> The years to act on. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime YearsFromNow(this int years)
        {
            return years.YearsFromNow(DateTime.Now);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that years from now. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="years"> The years to act on. </param>
        /// <param name="now">   The now Date/Time. </param>
        /// <returns> A DateTime. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime YearsFromNow(this int years, DateTime now)
        {
            return now.AddYears(years);
        }

        #endregion

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Iterates between integers from start to end and incrementing by the step at each
        ///           iteration. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="start"> The start. </param>
        /// <param name="end">   The end. </param>
        /// <param name="step">  The step. </param>
        /// <returns> An enumeration of all the steps from start to end. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<int> To(this int start, int end, int step = 1)
        {
            int currentIteration = start;

            while (currentIteration <= end)
            {
                yield return currentIteration;

                currentIteration += step;
            }
        }

        private static readonly string[] zeroOne = { "0", "1" };    /* The zero one */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts this IntegerExtensions to a bin. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value">  The value to act on. </param>
        /// <param name="length"> The length. </param>
        /// <returns> The given data converted to a string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToBinary(this int value, int length)
        {
            String s = "" + zeroOne[value & 1];
            int current = value;
            for (int i = length - 1; i > 0; i--)
            {
                current = current >> 1;
                s = zeroOne[current & 1] + s;
            }
            return s;
        }

        #region PercentageOf calculations

        /* Integer */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this int number, int percent)
        {
            return number * percent / 100;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this int number, int percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Long */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this int number, long percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this int number, long percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Float */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this int number, float percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this int number, float percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Double */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this int number, double percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this int number, double percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Decimal */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this int number, decimal percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this int number, decimal percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        #endregion

        #region AsPercentOf calculations

        /* Integer */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int AsPercentOf(this int number, int percent)
        {
            return number * 100 / percent;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this int number, int percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        /* Long */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int AsPercentOf(this int number, long percent)
        {
            return (int)(number * 100 / percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this int number, long percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        /* Float */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int AsPercentOf(this int number, float percent)
        {
            return (int)(number * 100 / percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this int number, float percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        /* Double */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int AsPercentOf(this int number, double percent)
        {
            return (int)(number * 100 / percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this int number, double percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        /* Decimal */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int AsPercentOf(this int number, decimal percent)
        {
            return (int)(number * 100 / percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this int number, decimal percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        #endregion

        #region Conversions

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int? extension method that convert this IntegerExtensions into a string representation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value">        The value to act on. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> A string that represents this IntegerExtensions. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToString(this int? value, string defaultvalue)
        {
            if (value == null) return defaultvalue;
            return value.Value.ToString();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts a l to an int. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a long. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static long ToInt(this int l) { return (long)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts a l to a decimal. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal ToDecimal(this int l) { return (decimal)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts a l to a float. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a float. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static float ToFloat(this int l) { return (float)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts a l to a double. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this int l) { return (double)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts a l to a byte. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a byte. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static byte ToByte(this int l) { return (byte)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that converts a l to a short. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a short. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static short ToShort(this int l) { return (short)l; }

        #endregion
    }
}
