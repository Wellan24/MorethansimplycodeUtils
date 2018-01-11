using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cartif.Extensions;

namespace Cartif.Expectation
{
    public static class FastExpectationExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be null. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static FastExpectation<T> Null<T>(this FastExpectation<T> exp)
        {
            return exp.Apply(t => t == null);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be the same as. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="others"> A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static FastExpectation<T> TheSameAs<T>(this FastExpectation<T> exp, params T[] others)
        {
            return exp.Apply((t, o) => Object.ReferenceEquals(t, o), others);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be equal to. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="others"> A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static FastExpectation<T> EqualTo<T>(this FastExpectation<T> exp, params T[] others)
        {
            return exp.Apply((t, o) => t.Equals(o), others);
        }

        public static FastExpectation<T> In<T>(this FastExpectation<T> exp, params T[] others)
        {
            return exp.Apply((t, o) => o.Contains(t), others);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be an instance of the given other. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="other"> The other. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static FastExpectation<T> AnInstanceOf<T>(this FastExpectation<T> exp, Type other)
        {
            return exp.Apply(t => t.GetType() == other);
        }

        public static FastExpectation<String> Empty(this FastExpectation<String> expectation)
        {
            return expectation.Apply(t => t.IsNullOrEmpty());
        }

        public static FastExpectation<IEnumerable<T>> Empty<T>(this FastExpectation<IEnumerable<T>> expectation)
        {
            return expectation.Apply(t => t.IsEmpty());
        }
    }
}
