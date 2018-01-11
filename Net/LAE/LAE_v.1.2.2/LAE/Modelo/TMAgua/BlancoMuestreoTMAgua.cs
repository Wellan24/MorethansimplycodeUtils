using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaBlancomuestreoTMAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("blancomuestreo_tmagua")]
    public class BlancomuestreoTMAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_blancomuestreoagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("conforme_blancomuestreoagua")]
        public Boolean Conforme { get; set; }

        [ColumnProperties("idparametro_blancomuestreoagua")]
        public int IdParametro { get; set; }

        [ColumnProperties("idtomamuestra_blancomuestreoagua")]
        public int IdTomaMuestra { get; set; }
    }
}
