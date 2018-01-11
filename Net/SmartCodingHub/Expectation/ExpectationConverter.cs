using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Expectation
{
    public class ExpectationConverter<T>
    {
        private AbstractExpectation<T> source;

        public ExpectationConverter(AbstractExpectation<T> source)
        {
            this.source = source;
        }

        public AbstractExpectation<T> To<TOther>() where TOther : AbstractExpectation<T>
        {
            TOther newExpectation = (TOther)Activator.CreateInstance(typeof(TOther), source);
            return newExpectation;
        }

        public AbstractExpectation<T> ToInmutable()
        {
            return new InmutableExpectation<T>(source);
        }

        public AbstractExpectation<T> ToCompiled()
        {
            return new CompiledExpectation<T>(source);
        }
    }
}
