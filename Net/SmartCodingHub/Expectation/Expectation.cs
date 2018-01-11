using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Expectation
{
    public static class Expectation<T>
    {
        private const int DEFAULT_CRITERIA_SIZE = 2;

        public static ExpectationBuilder<T> Build(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, true); }

        public static AbstractExpectation<T> ShouldBe(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, true).Compiled(); }
        public static AbstractExpectation<T> ShouldNotBe(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, false).Compiled(); }

        public static AbstractExpectation<T> CompiledShouldBe(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, true).Compiled(); }
        public static AbstractExpectation<T> CompiledShouldNotBe(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, false).Compiled(); }

        public static AbstractExpectation<T> InmutableShouldBe(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, true).Inmutable(); }
        public static AbstractExpectation<T> InmutableShouldNotBe(int capacity = DEFAULT_CRITERIA_SIZE) { return new ExpectationBuilder<T>(capacity, false).Inmutable(); }

        public static FastExpectation<T> FastShouldBe(T target) { return new ExpectationBuilder<T>(true).Fast(target); }
        public static FastExpectation<T> FastShouldNotBe(T target) { return new ExpectationBuilder<T>(false).Fast(target); }
    }

}
