using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Decimal Extensions like percentage of. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class DecimalExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Enumerates to in this collection. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="start"> The start to act on. </param>
        /// <param name="end">   The end. </param>
        /// <param name="step">  Amount to increment by. </param>
        /// <returns> An enumerator that allows foreach to be used to process to in this collection. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<decimal> To(this decimal start, decimal end, decimal step = 1)
        {
            decimal currentIteration = start;

            while (currentIteration <= end)
            {
                yield return currentIteration;

                currentIteration += step;
            }
        }

        #region PercentageOf calculations

        /* Integer */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this decimal number, int percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this decimal number, int percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Long */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this decimal number, long percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this decimal number, long percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Float */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this decimal number, float percent)
        {
            return (int)(number * (decimal)percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this decimal number, float percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Double */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this decimal number, double percent)
        {
            return (int)(number * (decimal)percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this decimal number, double percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Decimal */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this decimal number, decimal percent)
        {
            return (int)((decimal)number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this decimal number, decimal percent)
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
        public static int AsPercentOf(this decimal number, int percent)
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
        public static decimal AsDecimalPercentageOf(this decimal number, int percent)
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
        public static int AsPercentOf(this decimal number, long percent)
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
        public static decimal AsDecimalPercentageOf(this decimal number, long percent)
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
        public static int AsPercentOf(this decimal number, float percent)
        {
            return (int)(number * 100 / (decimal)percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this decimal number, float percent)
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
        public static int AsPercentOf(this decimal number, double percent)
        {
            return (int)(number * 100 / (decimal)percent);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An int extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal AsDecimalPercentageOf(this decimal number, double percent)
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
        public static int AsPercentOf(this decimal number, decimal percent)
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
        public static decimal AsDecimalPercentageOf(this decimal number, decimal percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        #endregion

        #region Conversions

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that converts a l to an int. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as an int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInt(this decimal l) { return (int)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that converts a l to a decimal. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal ToDecimal(this decimal l) { return (decimal)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that converts a l to a float. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a float. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static float ToFloat(this decimal l) { return (float)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that converts a l to a double. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this decimal l) { return (double)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that converts a l to a byte. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a byte. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static byte ToByte(this decimal l) { return (byte)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A decimal extension method that converts a l to a short. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a short. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static short ToShort(this decimal l) { return (short)l; }

        #endregion
    }
}
