using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaParametroMaterialPNT
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("parametro_materialpnt")]
    public class ParametroMaterialPNT : PersistenceData
    {
        [ColumnProperties("id_parametromaterialpnt", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idparametro_parametromaterialpnt")]
        public int? IdParametro { get; set; }

        [ColumnProperties("idmaterial_parametromaterialpnt")]
        public int? IdMaterial { get; set; }
    }
}