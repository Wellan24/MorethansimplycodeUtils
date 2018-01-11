using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
                /* c#5 */
                if (Expresion != null)
                    if (Expresion.Invoke(target, default(TParams)) != ShouldBeTrue)
                        return false;
                /* C#6*/
                //return Expresion?.Invoke(target, default(TParams)) == ShouldBeTrue;
            }
            else
            {
                foreach (TParams param in Params)
                {
                    /* c#5 */
                    if(Expresion!=null)
                        if(Expresion.Invoke(target, param)!=ShouldBeTrue)
                            return false;
                    

                    /* C#6*/
                    //if (Expresion?.Invoke(target, param) != ShouldBeTrue)
                    //    return false;
                }
            }

            return true;
        }
    }
}
