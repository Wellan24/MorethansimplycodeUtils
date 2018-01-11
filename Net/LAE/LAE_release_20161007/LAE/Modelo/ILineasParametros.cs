using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaILineasParametros
    {
        public static ILineasParametros GetILineaParametro(LineaRevisionOferta l)
        {
            return new ILineasParametros {
                Id=l.Id,
                Cantidad=l.Cantidad,
                IdParametro=l.IdParametro,
                IdRelacion=l.IdPControlRevisionOferta
            };
        }

        public static ILineasParametros GetILineaParametro(LineaPeticion l)
        {
            return new ILineasParametros
            {
                Id = l.Id,
                Cantidad = l.Cantidad,
                IdParametro = l.IdParametro,
                IdRelacion = l.IdPControlPeticion
            };
        }
    }

    public class ILineasParametros
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int IdParametro { get; set; }
        public int IdRelacion { get; set; }
    }
}
