using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cartif.Expectation
{
    public class ExpectationCriteria<T, TParams>
    {
        public Boolean Any { get; set; }
        public Boolean ShouldBeTrue { get; set; }
        public Func<T, TParams, Boolean> Expresion { get; set; }
        public TParams[] Params { get; set; }

        public ExpectationCriteria()
        {
            Any = false;
            ShouldBeTrue = true;
        }

        public ExpectationCriteria(Boolean any, Boolean shouldBeTrue, Func<T, TParams, Boolean> expresion, TParams[] parameters)
        {
            Any = any;
            ShouldBeTrue = shouldBeTrue;
            Expresion = expresion;
            Params = parameters;
        }

        public Boolean Invoke(T target)
        {
            if (Params == null)
            {
                /* c#5 */
                if (Expresion != null)
                    if (Expresion.Invoke(target, default(TParams)) != ShouldBeTrue)
                        return false;
            }
            else if (Any)
            {
                /* If the expression is correct for any of the params, return true, false otherwise */
                foreach (TParams param in Params)
                {
                    if (Expresion?.Invoke(target, param) == ShouldBeTrue)
                        return true;
                }
                return false;
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
