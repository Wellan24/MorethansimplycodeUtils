using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Extension Methods to be used by Objects as a check for Null. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class ObjectExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Throws an exception if the object called upon is null. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <typeparam name="T"> The calling class. </typeparam>
        /// <param name="obj">  The This object. </param>
        /// <param name="text"> The text to be written on the ArgumentNullException: [text] not allowed to be
        ///                     null. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ThrowIfArgumentIsNull<T>(this T obj, string text) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(text + " not allowed to be null");
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An object extension method that query if '@object' is null. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="object"> The @object to act on. </param>
        /// <returns> true if null, false if not. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsNull(this object @object)
        {
            return ReferenceEquals(@object, null);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> An object extension method that query if '@object' is not null. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="object"> The @object to act on. </param>
        /// <returns> true if not null, false if not. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsNotNull(this object @object)
        {
            return !@object.IsNull();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets the default. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <param name="type"> The type. </param>
        /// <returns> The default. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }

    
}
