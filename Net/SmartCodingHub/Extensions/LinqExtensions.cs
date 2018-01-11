using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A linq extensions methods, as ToCSV and ForEach. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class LinqExtensions
    {
        #region ToCSV

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A System.Linq.IOrderedQueryable extension method that converts a data to a CSV string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="data"> The data. </param>
        /// <returns> data as a string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToCSVString<T>(this IEnumerable<T> data)
        {
            return ToCSVString(data, ";");
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A System.Linq.IOrderedQueryable extension method that converts this LinqExtensions to a
        ///           CSV string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="data">      The data. </param>
        /// <param name="delimiter"> The delimiter. </param>
        /// <returns> The given data converted to a string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToCSVString<T>(this IEnumerable<T> data, string delimiter)
        {
            return ToCSVString(data, delimiter, ":");
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A System.Linq.IOrderedQueryable extension method that converts this LinqExtensions to a
        ///           CSV string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="data">             The data. </param>
        /// <param name="delimiter">        The delimiter. </param>
        /// <param name="replaceDelimiter"> The replace delimiter. </param>
        /// <returns> The given data converted to a string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToCSVString<T>(this IEnumerable<T> data, string delimiter, string replaceDelimiter)
        {
            return ToCSVString(data, delimiter, replaceDelimiter, null, false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A System.Linq.IOrderedQueryable extension method that converts this LinqExtensions to a
        ///           CSV string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="data">              The data. </param>
        /// <param name="delimiter">         The delimiter. </param>
        /// <param name="replaceDelimiter">  The replace delimiter. </param>
        /// <param name="nullvalue">         The nullvalue. </param>
        /// <param name="replaceUnderscore"> true to replace underscore. </param>
        /// <returns> The given data converted to a string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToCSVString<T>(this IEnumerable<T> data, string delimiter, string replaceDelimiter, string nullvalue, Boolean replaceUnderscore)
        {
            StringBuilder csvdata = new StringBuilder();
            string replaceFrom = delimiter.Trim();

            IQueryable<T> query = data.AsQueryable();
            System.Reflection.PropertyInfo[] headers = query.ElementType.GetProperties();

            if (replaceFrom == replaceDelimiter)
                replaceDelimiter = (replaceFrom != "_") ? "_" : ":";

            if (headers.Length > 0)
            {
                foreach (var head in headers)
                    csvdata.Append(head.Name.Replace("_", " ") + delimiter);

                csvdata.Append("\n");
            }

            foreach (var row in data)
            {
                var fields = row.GetType().GetProperties();
                for (int i = 0; i < fields.Length; i++)
                {
                    object value = null;
                    try
                    {
                        value = fields[i].GetValue(row, null);
                    }
                    catch { /* Empty */ }
                    if (value != null)
                    {
                        csvdata.Append(value.ToString().Replace("\r", "\f").Replace("\n", " \f").Replace("_", " ").Replace(replaceFrom, replaceDelimiter) + delimiter);
                    }
                    else
                    {
                        csvdata.Append(nullvalue);
                        csvdata.Append(delimiter);
                    }
                }
                csvdata.Append("\n");
            }

            return csvdata.ToString();
        }

        #endregion

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;T&gt; extension method that query if 'enumeration' is empty. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <returns> true if empty, false if not. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsEmpty<T>(this IEnumerable<T> enumeration)
        {
            return enumeration == null || enumeration.FirstOrDefault() == null;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;T&gt; extension method that each with index. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="action">     The action. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ForEachWithIndex<T>(this IEnumerable<T> enumeration, Action<long, T> action)
        {
            action.ThrowIfArgumentIsNull("action");
            enumeration.ThrowIfArgumentIsNull("enumeration");

            long index = 0;
            foreach (var element in enumeration)
            {
                action.Invoke(index, element);
                index++;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Enumerates for each in this collection. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="enumeration"> The enumeration to act on. </param>
        /// <param name="action">      The action. </param>
        /// <returns> An enumerator that allows foreach to be used to process for each in this collection. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            action.ThrowIfArgumentIsNull("action");
            enumeration.ThrowIfArgumentIsNull("enumeration");

            int i = 0;
            foreach (T item in enumeration)
            {
                if (item == null)
                    throw new NullReferenceException("item at pos[" + i + "] is null");

                action(item);
                i++;
            }

            return enumeration;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Enumerates for each in this collection. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="enumeration"> The enumeration to act on. </param>
        /// <param name="action">      The action. </param>
        /// <returns> An enumerator that allows foreach to be used to process for each in this collection. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IOrderedEnumerable<T> ForEach<T>(this IOrderedEnumerable<T> enumeration, Action<T> action)
        {
            action.ThrowIfArgumentIsNull("action");
            enumeration.ThrowIfArgumentIsNull("enumeration");

            int i = 0;
            foreach (T item in enumeration)
            {
                if (item == null)
                    throw new NullReferenceException("item at pos[" + i + "] is null");

                action(item);
                i++;
            }

            return enumeration;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;T&gt; extension method that join as string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="collection">     The enumeration to act on. </param>
        /// <param name="separator">      The separator. </param>
        /// <param name="oneItemPerLine"> true to one item per line. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string JoinAsString<T>(this IEnumerable<T> enumeration, string separator, bool oneItemPerLine)
        {
            if (enumeration.IsEmpty())
                return String.Empty;

            var builder = new StringBuilder();
            var first = true;

            foreach (var element in enumeration)
            {
                if (!first)
                {
                    builder.Append(separator);
                    if (oneItemPerLine)
                    {
                        builder.AppendLine();
                    }
                }
                builder.Append(element);
                first = false;
            }

            return builder.ToString();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;T&gt; extension method that join as string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="separator">  The separator. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string JoinAsString<T>(this IEnumerable<T> enumeration, string separator)
        {
            return enumeration.JoinAsString(separator, false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;TOrigin&gt; extension method that collects. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOrigin"> Type of the origin. </typeparam>
        /// <typeparam name="TReturn"> Type of the return. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="function">   The function. </param>
        /// <returns> A List&lt;TReturn&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<TReturn> Collect<TOrigin, TReturn>(
            this IEnumerable<TOrigin> enumeration, Func<TOrigin, TReturn> func)
        {
            return enumeration.Map(func);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;TOrigin&gt; extension method that maps. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOrigin"> Type of the origin. </typeparam>
        /// <typeparam name="TReturn"> Type of the return. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="function">   The function. </param>
        /// <returns> A List&lt;TReturn&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static IEnumerable<TReturn> Map<TOrigin, TReturn>(
           this IEnumerable<TOrigin> enumeration, Func<TOrigin, TReturn> func)
        {
            return enumeration.Select(element => func(element));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;TOrigin&gt; extension method that reduces. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOrigin"> Type of the origin. </typeparam>
        /// <typeparam name="TReturn"> Type of the return. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="base">       The base. </param>
        /// <param name="function">   The function. </param>
        /// <returns> A TReturn. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TReturn Reduce<TOrigin, TReturn>(
            this IEnumerable<TOrigin> enumeration, TReturn @base, Func<TReturn, TOrigin, TReturn> func)
        {
            return enumeration.Inject(@base, func);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;TOrigin&gt; extension method that reduces. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOrigin"> Type of the origin. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="function">   The function. </param>
        /// <returns> A TOrigin. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TOrigin Reduce<TOrigin>(
            this IEnumerable<TOrigin> enumeration, Func<TOrigin, TOrigin, TOrigin> func)
        {
            return enumeration.Inject(func);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;TOrigin&gt; extension method that injects. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOrigin"> Type of the origin. </typeparam>
        /// <typeparam name="TReturn"> Type of the return. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="base">       The base. </param>
        /// <param name="function">   The function. </param>
        /// <returns> A TReturn. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TReturn Inject<TOrigin, TReturn>(
            this IEnumerable<TOrigin> enumeration, TReturn @base, Func<TReturn, TOrigin, TReturn> func)
        {
            if (enumeration.IsEmpty())
                return @base;

            var iteration = func(@base, enumeration.First());

            return enumeration.Skip(1).Inject(iteration, func);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An IEnumerable&lt;TOrigin&gt; extension method that injects. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOrigin"> Type of the origin. </typeparam>
        /// <param name="collection"> The enumeration to act on. </param>
        /// <param name="function">   The function. </param>
        /// <returns> A TOrigin. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TOrigin Inject<TOrigin>(
            this IEnumerable<TOrigin> enumeration, Func<TOrigin, TOrigin, TOrigin> func)
        {
            return enumeration.IsEmpty() ? default(TOrigin) : enumeration.Inject(enumeration.First(), func);
        }
    }
}
