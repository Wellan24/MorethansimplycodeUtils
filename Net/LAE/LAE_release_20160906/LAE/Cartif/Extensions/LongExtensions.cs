using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Long Extensions like percentage of. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class LongExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Iterates between longs from start to end and incrementing by the step at each iteration. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="start"> The start. </param>
        /// <param name="end">   The end. </param>
        /// <param name="step">  The step. </param>
        /// <returns> An enumeration of all the steps from start to end. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<long> To(this long start, long end, long step = 1)
        {
            long currentIteration = start;

            while (currentIteration <= end)
            {
                yield return currentIteration;

                currentIteration += step;
            }
        }

        #region PercentageOf calculations

        /* Integer */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this long number, int percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this long number, int percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Long */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this long number, long percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this long number, long percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Float */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this long number, float percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this long number, float percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Double */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this long number, double percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this long number, double percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Decimal */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this long number, decimal percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this long number, decimal percent)
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
        public static int AsPercentOf(this long number, int percent)
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
        public static decimal AsDecimalPercentageOf(this long number, int percent)
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
        public static int AsPercentOf(this long number, long percent)
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
        public static decimal AsDecimalPercentageOf(this long number, long percent)
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
        public static int AsPercentOf(this long number, float percent)
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
        public static decimal AsDecimalPercentageOf(this long number, float percent)
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
        public static int AsPercentOf(this long number, double percent)
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
        public static decimal AsDecimalPercentageOf(this long number, double percent)
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
        public static int AsPercentOf(this long number, decimal percent)
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
        public static decimal AsDecimalPercentageOf(this long number, decimal percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        #endregion

        #region Conversions

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that converts a l to an int. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as an int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInt(this long l) { return (int)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that converts a l to a decimal. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal ToDecimal(this long l) { return (decimal)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that converts a l to a float. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a float. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static float ToFloat(this long l) { return (float)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that converts a l to a double. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this long l) { return (double)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that converts a l to a byte. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a byte. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static byte ToByte(this long l) { return (byte)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A long extension method that converts a l to a double. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a short. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static short ToShort(this long l) { return (short)l; }

        #endregion
    }
}
