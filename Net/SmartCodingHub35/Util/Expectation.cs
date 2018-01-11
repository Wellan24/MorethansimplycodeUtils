using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cartif.Extensions;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An expectation. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class Expectation<T>
    {
        #region Static Initializers

        public static Expectation<T> Should(int capacity = 4) { return new Expectation<T>(capacity, true); }
        public static Expectation<T> ShouldNot(int capacity = 4) { return new Expectation<T>(capacity, false); }

        #endregion

        #region Fields

        private Boolean currentTruthness;  /* true to truthness */
        private ExpectationExpresion<T, Object>[] expresions = null;

        #endregion

        #region Constructors

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="truthness"> true to truthness. </param>
        /// <param name="target">    Target for the. </param>
        ///--------------------------------------------------------------------------------------------------
        public Expectation(Boolean truthness = true) : this(4, truthness) { }

        public Expectation(int expectedExpresions, Boolean truthness = true)
        {
            this.currentTruthness = truthness;
            expresions = new ExpectationExpresion<T, Object>[expectedExpresions];
        }

        #endregion

        #region Change Truthness

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Following should not be true. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public Expectation<T> FollowingShould()
        {
            currentTruthness = true;
            return this;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Following should not be false. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public Expectation<T> FollowingShouldNot()
        {
            currentTruthness = false;
            return this;
        }

        #endregion

        #region Check function

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Query if this Expectation&lt;T&gt; is what expected. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <returns> true if what expected, false if not. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean Check(T target)
        {
            int length = expresions.Length;
            ExpectationExpresion<T, Object> expresion = null;
            Boolean expectationIsFulfilled = true;
            for (int i = 0; i < length; i++)
            {
                expresion = expresions[i];
                /* C#5 */
                if(expresion!=null)
                    expectationIsFulfilled = expectationIsFulfilled && expresion.Invoke(target);
                /* C#6 */
                //expectationIsFulfilled = expectationIsFulfilled && (expresion?.Invoke(target) ?? true);

                if (!expectationIsFulfilled)
                    return false;
            }

            return true;

        }

        #endregion

        #region Utility methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Applies this Expectation&lt;T&gt; </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TOther"> Type of the other. </typeparam>
        /// <param name="matchFunction"> The match action. </param>
        /// <param name="others">        A variable-length parameters list containing others. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public Expectation<T> AddTest<TOther>(Func<T, TOther, Boolean> matchFunction, params TOther[] others)
        {
            //expectationIsTrue = expectationIsTrue && others.All(other => matchFunction(target, other)) == truthness;

            matchFunction.ThrowIfArgumentIsNull("You need a function to check");
            others.ThrowIfArgumentIsNull("You need a parameters list");

            expresions.InsertInEmptySpace(new ExpectationExpresion<T, Object>
            {
                ShouldBeTrue = currentTruthness,
                Expresion = (target, param) => matchFunction(target, (TOther)param),
                Params = others.Map(o => (Object)o).ToArray()
            });

            return this;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Applies the given matchAction. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="matchFunction"> The match action. </param>
        /// <returns> An Expectation&lt;T&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public Expectation<T> AddTest(Func<T, Boolean> matchFunction)
        {
            matchFunction.ThrowIfArgumentIsNull("You need a function to check");

            expresions.InsertInEmptySpace(new ExpectationExpresion<T, Object>
            {
                ShouldBeTrue = currentTruthness,
                Expresion = (target, param) => matchFunction(target),
                Params = null
            });
            return this;
        }

        #endregion
    }
}