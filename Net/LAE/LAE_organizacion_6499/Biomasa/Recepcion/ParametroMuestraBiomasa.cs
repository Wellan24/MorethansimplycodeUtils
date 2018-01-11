using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaParametrosMuestrabiomasa
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("parametros_muestrabiomasa")]
    public class ParametroMuestraBiomasa : PersistenceData, IModelo
    {
        [ColumnProperties("id_parametromuestrabiomasa", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idmuestra_parametromuestrabiomasa")]
        public int IdMuestra { get; set; }

        [ColumnProperties("idprocedimiento_parametromuestrabiomasa")]
        public int IdProcedimiento { get; set; }
    }
}