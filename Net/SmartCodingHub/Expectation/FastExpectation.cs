using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cartif.Expectation
{
    public class FastExpectation<T>
    {
        #region Static Methods

        #endregion


        private readonly T target;  /* Target for the */
        private Boolean truthness;  /* true to truthness */
        private Boolean evaluated;  /* true if expectation is true */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="truthness"> true to truthness. </param>
        /// <param name="target">    Target for the. </param>
        ///--------------------------------------------------------------------------------------------------
        public FastExpectation(Boolean truthness, T target)
        {
            this.truthness = truthness;
            this.target = target;
            this.evaluated = true;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Applies this Expectation&lt;T&gt; </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOther"> Type of the other. </typeparam>
        /// <param name="matchAction"> The match action. </param>
        /// <param name="others">      A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public FastExpectation<T> Apply<TOther>(Func<T, TOther, Boolean> matchAction, params TOther[] others)
        {
            evaluated = evaluated && others.All(other => matchAction(target, other)) == truthness;
            return this;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Applies the given matchAction. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="matchAction"> The match action. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public FastExpectation<T> Apply(Func<T, Boolean> matchAction)
        {
            evaluated = evaluated && matchAction(target) == truthness;
            return this;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Query if this Expectation&lt;T&gt; is what expected. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> true if what expected, false if not. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean Evaluate()
        {
            return evaluated;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Following should not be true. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public FastExpectation<T> Be()
        {
            truthness = true;
            return this;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Following should not be false. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public FastExpectation<T> NotBe()
        {
            truthness = false;
            return this;
        }

    }
}
