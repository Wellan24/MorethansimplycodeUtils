using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaLineaali_recepcionagua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("lineaali_recepcionagua")]
    public class LineaAliRecepcionAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_lineaalirecepcionagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idparametro_linealirecepcionagua")]
        public int IdParametro { get; set; }

        [ColumnProperties("idalicuota_linealirecepcionagua")]
        public int IdAlicuota { get; set; }
    }
}
