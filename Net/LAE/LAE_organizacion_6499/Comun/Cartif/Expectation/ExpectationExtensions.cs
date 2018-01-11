using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cartif.Expectation
{
    public static class ExpectationExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be null. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static AbstractExpectation<T> Null<T>(this AbstractExpectation<T> exp)
        {
            return exp.AddCriteria(t => t == null);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be the same as. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="others"> A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static AbstractExpectation<T> SameAs<T, TOther>(this AbstractExpectation<T> exp, params TOther[] others)
        {
            return exp.AddCriteria((t, o) => Object.ReferenceEquals(t, o), others);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be equal to. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="others"> A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static AbstractExpectation<T> Equal<T, TOther>(this AbstractExpectation<T> exp, params TOther[] others)
        {
            return exp.AddCriteria((t, o) => t.Equals(o), others);
        }

        public static AbstractExpectation<T> In<T>(this AbstractExpectation<T> exp, params T[] others)
        {
            return exp.AddCriteria((t, o) => o.Contains(t), others);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Be an instance of the given other. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="other"> The other. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public static AbstractExpectation<T> InstanceOf<T>(this AbstractExpectation<T> exp, Type other)
        {
            return exp.AddCriteria(t => t.GetType() == other);
        }

    }
}
