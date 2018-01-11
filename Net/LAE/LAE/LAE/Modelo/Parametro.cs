using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaParametros
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("parametros")]
    public class Parametro : PersistenceData
    {
        [ColumnProperties("id_parametro", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_parametro")]
        public String NombreParametro { get; set; }

        public override String ToString()
        {
            return NombreParametro;
        }
    }
}
