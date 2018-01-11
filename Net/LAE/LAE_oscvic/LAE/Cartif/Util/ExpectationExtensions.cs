using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Extensions
{
    public static class ExpectationExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be null. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Expectation<T> BeNull<T>(this Expectation<T> exp)
        {
            return exp.AddTest(t => t == null);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be the same as. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="others"> A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Expectation<T> BeTheSameAs<T>(this Expectation<T> exp, params T[] others)
        {
            return exp.AddTest((t, o) => Object.ReferenceEquals(t, o), others);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be equal to. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="others"> A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Expectation<T> BeEqualTo<T>(this Expectation<T> exp, params T[] others)
        {
            return exp.AddTest((t, o) => t.Equals(o), others);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be an instance of the given other. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="other"> The other. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Expectation<T> BeAnInstanceOf<T>(this Expectation<T> exp, Type other)
        {
            return exp.AddTest(t => t.GetType() == other);
        }

    }
}
