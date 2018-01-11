using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cartif.Extensions;

namespace Cartif.Expectation
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> This class uses the default implementation for all methods </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class CompiledExpectation<T> : AbstractExpectation<T>
    {
        public CompiledExpectation(Boolean truthness = true) : base(4, truthness) { }

        public CompiledExpectation(int expectedExpresions, Boolean truthness = true) : base(expectedExpresions, truthness) { }

        public CompiledExpectation(AbstractExpectation<T> expectation) : base(expectation) { }
    }
}