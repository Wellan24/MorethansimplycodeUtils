using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Util
{
    public class ExpectationExpresion<T, TParams>
    {
        public Boolean ShouldBeTrue { get; set; }
        public Func<T, TParams, Boolean> Expresion { get; set; }
        public TParams[] Params { get; set; }

        public ExpectationExpresion() { }

        public ExpectationExpresion(Boolean shouldBeTrue, Func<T, TParams, Boolean> expresion, TParams[] parameters)
        {
            ShouldBeTrue = shouldBeTrue;
            Expresion = expresion;
            Params = parameters;
        }

        public Boolean Invoke(T target)
        {
            if (Params == null)
            {
                return Expresion?.Invoke(target, default(TParams)) == ShouldBeTrue;
            }
            else
            {
                foreach (TParams param in Params)
                {
                    if (Expresion?.Invoke(target, param) != ShouldBeTrue)
                        return false;
                }
            }

            return true;
        }
    }
}
