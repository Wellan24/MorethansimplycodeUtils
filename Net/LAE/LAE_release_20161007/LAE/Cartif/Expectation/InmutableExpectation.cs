using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cartif.Extensions;

namespace Cartif.Expectation
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An expectation. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class InmutableExpectation<T> : AbstractExpectation<T>
    {
        #region Constructors

        public InmutableExpectation(Boolean truthness = true) : base(4, truthness) { }

        public InmutableExpectation(int expectedExpresions, Boolean truthness = true) : base(expectedExpresions, truthness) { }

        public InmutableExpectation(AbstractExpectation<T> expectation) : base(expectation) { }

        #endregion

        #region Change Truthness

        public override AbstractExpectation<T> Be() => new InmutableExpectation<T>(base.Be());

        public override AbstractExpectation<T> NotBe() => new InmutableExpectation<T>(base.NotBe());

        public override AbstractExpectation<T> MatchingAny() => new InmutableExpectation<T>(base.MatchingAny());

        public override AbstractExpectation<T> MatchingAll() => new InmutableExpectation<T>(base.MatchingAll());

        #endregion

        #region AddCriteria methods

        public override AbstractExpectation<T> AddCriteria(Func<T, Boolean> matchFunction) => new InmutableExpectation<T>(base.AddCriteria(matchFunction));

        public AbstractExpectation<T> AddCriteria<TOther>(Func<T, TOther, Boolean> matchFunction, params TOther[] others) =>
            new InmutableExpectation<T>(base.AddCriteria(matchFunction, others));

        #endregion

        #region Evaluate function

        public override Boolean Evaluate(T target) => base.Evaluate(target);

        #endregion
    }
}