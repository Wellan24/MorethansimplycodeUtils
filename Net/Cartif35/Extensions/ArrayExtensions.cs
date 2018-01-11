using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An array extensions. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class ArrayExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> A T[] extension method that inserts. Inserts in the specified index. Can throw an
        ///           exception if the index is not valid. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="xs"> The xs to act on. </param>
        /// <param name="i">  Zero-based index of the. </param>
        /// <param name="x">  The T to process. </param>
        /// <returns> A T[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T[] Insert<T>(this T[] xs, int i, T x)
        {
            var tmp = new T[xs.Length + 1];
            Array.Copy(xs, 0, tmp, 0, i);
            tmp[i] = x;
            Array.Copy(xs, i, tmp, i + 1, xs.Length - i);
            return tmp;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A T[] extension method that inserts. Inserts at the end. Can throw an exception if the
        ///           index is not valid (> int.Max). </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="array"> The array to act on. </param>
        /// <param name="obj">   The object. </param>
        /// <returns> A T[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T[] Insert<T>(this T[] array, T obj)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = obj;
            return array;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A T[] extension method that inserts. Inserts in the first place empty if no exists inserts at the end. Can throw an exception if the
        ///           index is not valid (> int.Max). </summary>
        /// <remarks> Manper, 2016-03-10. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="array"> The array to act on. </param>
        /// <param name="obj">   The object. </param>
        /// <returns> A T[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T[] InsertInEmptySpace<T>(this T[] array, T obj)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (array[i] == null)
                {
                    array[i] = obj;
                    return array;
                }
            }

            array = array.Insert(obj);
            return array;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A T[] extension method that removes the given index and redimensionates the de array. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="xs"> The xs to act on. </param>
        /// <param name="i">  Zero-based index of the. </param>
        /// <returns> A T[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T[] Remove<T>(this T[] xs, int i)
        {
            var n = xs.Length - 1;
            var tmp = new T[n];
            Array.Copy(xs, 0, tmp, 0, i);
            Array.Copy(xs, i + 1, tmp, i, n - i);
            return tmp;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A T[] extension method that cuts the array at $i and copies it in another array of the
        ///           length $len. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="xs">  The xs to act on. </param>
        /// <param name="i">   Zero-based index of the. </param>
        /// <param name="len"> The length. </param>
        /// <returns> A T[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T[] Slice<T>(this T[] xs, int i, int len)
        {
            var tmp = new T[len];
            Array.Copy(xs, i, tmp, 0, len);
            return tmp;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A T[] extension method that copies the given xs. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="xs"> The xs to act on. </param>
        /// <returns> A T[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T[] Copy<T>(this T[] xs)
        {
            var tmp = new T[xs.Length];
            Array.Copy(xs, 0, tmp, 0, xs.Length);
            return tmp;
        }
    }
}
