using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Float Extensions like percentage. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class FloatExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Enumerates to in this collection. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="start"> The start to act on. </param>
        /// <param name="end">   The end. </param>
        /// <param name="step">  Amount to increment by. </param>
        /// <returns> An enumerator that allows foreach to be used to process to in this collection. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<float> To(this float start, float end, float step = 1)
        {
            float currentIteration = start;

            while (currentIteration <= end)
            {
                yield return currentIteration;

                currentIteration += step;
            }
        }

        #region PercentageOf calculations

        /* Integer */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this float number, int percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this float number, int percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Long */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this float number, long percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this float number, long percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Float */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this float number, float percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this float number, float percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Double */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this float number, double percent)
        {
            return (int)(number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this float number, double percent)
        {
            return (decimal)number / 100 * (decimal)percent;
        }

        /* Decimal */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int PercentageOf(this float number, decimal percent)
        {
            return (int)((decimal)number * percent / 100);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that decimal percentage of. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="number">  The number to act on. </param>
        /// <param name="percent"> The percent. </param>
        /// <returns> A decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal DecimalPercentageOf(this float number, decimal percent)
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
        public static int AsPercentOf(this float number, int percent)
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
        public static decimal AsDecimalPercentageOf(this float number, int percent)
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
        public static int AsPercentOf(this float number, long percent)
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
        public static decimal AsDecimalPercentageOf(this float number, long percent)
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
        public static int AsPercentOf(this float number, float percent)
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
        public static decimal AsDecimalPercentageOf(this float number, float percent)
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
        public static int AsPercentOf(this float number, double percent)
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
        public static decimal AsDecimalPercentageOf(this float number, double percent)
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
        public static int AsPercentOf(this float number, decimal percent)
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
        public static decimal AsDecimalPercentageOf(this float number, decimal percent)
        {
            return (decimal)number * 100 / (decimal)percent;
        }

        #endregion

        #region Conversions

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that converts a l to an int. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as an int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInt(this float l) { return (int)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that converts a l to a decimal. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a decimal. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static decimal ToDecimal(this float l) { return (decimal)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that converts a l to a float. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a float. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static float ToFloat(this float l) { return (float)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that converts a l to a double. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this float l) { return (double)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that converts a l to a byte. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a byte. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static byte ToByte(this float l) { return (byte)l; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A float extension method that converts a l to a short. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The l to act on. </param>
        /// <returns> l as a short. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static short ToShort(this float l) { return (short)l; }

        #endregion

        #region IsNaN

        /// <summary>
        /// It is 5 times faster than float.IsNaN. Stackoverflow: http://stackoverflow.com/questions/639010/how-can-i-compare-a-float-to-nan-if-comparisons-to-nan-always-return-false
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        //public static unsafe bool IsNaNUnsafe(float f)
        //{
        //    int binary = *(int*)(&f);
        //    return ((binary & 0x7F800000) == 0x7F800000) && ((binary & 0x007FFFFF) != 0);
        //}

        ///--------------------------------------------------------------------------------------------------
        /// <summary> It's still 3 times faster than IsNaN. But don't uses unsafe code. Stackoverflow:
        ///           http://stackoverflow.com/questions/639010/how-can-i-compare-a-float-to-nan-if-
        ///           comparisons-to-nan-always-return-false. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="f"> . </param>
        /// <returns> true if NaN safe, false if not. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsNaNSafe(this float f)
        {
            FloatUnion union = new FloatUnion();
            union.value = f;

            return ((union.binary & 0x7F800000) == 0x7F800000) && ((union.binary & 0x007FFFFF) != 0);
        }

        #endregion
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A float union. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    [StructLayout(LayoutKind.Explicit)]
    public struct FloatUnion
    {
        [FieldOffset(0)]
        public float value; /* The value */

        [FieldOffset(0)]
        public int binary;  /* The binary */
    }
}
