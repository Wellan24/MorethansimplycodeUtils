using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Calculos
{
    public class RegresionLineal
    {
        public double? Pendiente { get; set; }
        public double? Interseccion { get; set; }

        public double GetY(double x)
        {
            return Interseccion ?? 0 + x * Pendiente ?? 0;
        }
    }
}
