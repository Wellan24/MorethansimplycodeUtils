using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Expectation
{
    public class ExpectationBuilder<T>
    {
        private int capacity;
        private Boolean truthness;

        public ExpectationBuilder(Boolean truthness) : this(0, truthness) { }

        public ExpectationBuilder(int capacity, Boolean truthness)
        {
            this.capacity = capacity;
            this.truthness = truthness;
        }

        public ExpectationBuilder<T> Should()
        {
            this.truthness = true;
            return this;
        }

        public ExpectationBuilder<T> ShouldNot()
        {
            this.truthness = false;
            return this;
        }

        public ExpectationBuilder<T> Capacity(int capacity)
        {
            this.capacity = capacity;
            return this;
        }

        public AbstractExpectation<T> Compiled()
        {
            return new CompiledExpectation<T>(capacity, truthness);
        }

        public AbstractExpectation<T> Inmutable()
        {
            return new InmutableExpectation<T>(capacity, truthness);
        }

        public FastExpectation<T> Fast(T target)
        {
            return new FastExpectation<T>(truthness, target);
        }
    }
}
