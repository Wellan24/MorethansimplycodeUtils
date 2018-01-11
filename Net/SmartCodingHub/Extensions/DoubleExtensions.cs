using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Double Extensions like percentage of. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class DoubleExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Enumerates to in this collection. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="start"> The start to act on. </param>
        /// <param name="end">   The end. </param>
        /// <param name="step">  Amount to increment by. </param>
        /// <returns> An enumerator that allows foreach to be used to process to in this collection. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<double> To(this double start, double end, double step = 1)
        {
            double currentIteration = start;

            while (currentIteration <= end)
            {
                yield return currentIteration;

                currentIteration += step;
            }
        }

        #region PercentageOf calculations

        /* Integer */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this double number, int percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this double number, int percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Long */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this double number, long percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this double number, long percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Float */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this double number, float percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this double number, float percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Double */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this double number, double percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this double number, double percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Decimal */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this double number, decimal percent)
        {
            return (int)((decimal)number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this double number, decimal percent)
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
        public static int AsPercentOf(this double number, int percent)
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
        public static decimal AsDecimalPercentageOf(this double number, int percent)
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
        public static int AsPercentOf(this double number, long percent)
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
        public static decimal AsDecimalPercentageOf(this double number, long percent)
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
        public static int AsPercentOf(this double number, float percent)
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
        public static decimal AsDecimalPercentageOf(this double number, float percent)
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
        public static int AsPercentOf(this double number, double percent)
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
        public static decimal AsDecimalPercentageOf(this double number, double percent)
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
        public static int AsPercentOf(this double number, decimal percent)
        {
            return (int)((decimal)number * 100 / percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this double number, decimal percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        #endregion

        #region Conversions

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that converts a l to an int. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as an int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInt(this double l) { return (int)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that converts a l to a decimal. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal ToDecimal(this double l) { return (decimal)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that converts a l to a float. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a float. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static float ToFloat(this double l) { return (float)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that converts a l to a double. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this double l) { return (double)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that converts a l to a byte. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a byte. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static byte ToByte(this double l) { return (byte)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A double extension method that converts a l to a short. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a short. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static short ToShort(this double l) { return (short)l; }

        #endregion
    }
}
