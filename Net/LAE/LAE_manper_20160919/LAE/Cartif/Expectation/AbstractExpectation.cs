using Cartif.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Expectation
{
    public abstract class AbstractExpectation<T>
    {
        #region Properties

        public virtual Boolean Truthness { get; set; }
        public virtual Boolean Any { get; set; }
        public virtual ExpectationCriteria<T, Object>[] Criteria { get; protected set; }

        #endregion

        #region Constructors

        public AbstractExpectation() : this(4, true) { }

        public AbstractExpectation(Boolean truthness) : this(4, truthness) { }

        public AbstractExpectation(int expectedExpresions, Boolean truthness)
        {
            Truthness = truthness;
            Criteria = new ExpectationCriteria<T, Object>[expectedExpresions];
        }

        public AbstractExpectation(AbstractExpectation<T> expectation)
        {
            Any = expectation.Any;
            Truthness = expectation.Truthness;
            Criteria = expectation.Criteria.Copy();
        }

        #endregion

        #region Change Truthness and Any

        public virtual AbstractExpectation<T> Be()
        {
            Truthness = true;
            return this;
        }

        public virtual AbstractExpectation<T> NotBe()
        {
            Truthness = false;
            return this;
        }

        public virtual AbstractExpectation<T> MatchingAny()
        {
            Any = true;
            return this;
        }

        public virtual AbstractExpectation<T> MatchingAll()
        {
            Any = false;
            return this;
        }

        #endregion

        #region AddCriteria methods

        public virtual AbstractExpectation<T> AddCriteria(Func<T, Boolean> matchFunction)
        {
            matchFunction.ThrowIfArgumentIsNull("You need a function to check");

            Criteria = Criteria.InsertInEmptySpace(new ExpectationCriteria<T, Object>
            {
                ShouldBeTrue = Truthness,
                Expresion = (target, param) => matchFunction(target),
                Params = null
            });

            return this;
        }

        public virtual AbstractExpectation<T> AddCriteria<TOther>(Func<T, TOther, Boolean> matchFunction, params TOther[] others)
        {
            matchFunction.ThrowIfArgumentIsNull("You need a function to check");
            others.ThrowIfArgumentIsNull("You need a parameters list");

            Criteria = Criteria.InsertInEmptySpace(new ExpectationCriteria<T, Object>
            {
                Any = Any,
                ShouldBeTrue = Truthness,
                Expresion = (target, param) => matchFunction(target, (TOther)param),
                Params = others.Map(o => (Object)o).ToArray()
            });

            return this;
        }

        #endregion

        #region Evaluate function

        public virtual Boolean Evaluate(T target)
        {
            foreach (ExpectationCriteria<T, Object> expCriteria in Criteria)
            {
                if (!(expCriteria?.Invoke(target) ?? true))
                    return false;
            }

            return true;
        }

        #endregion

        #region Convert

        public ExpectationConverter<T> Convert()
        {
            return new ExpectationConverter<T>(this);
        }

        #endregion
    }
}
