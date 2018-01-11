using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaChnConstantes
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.chn_constantes")]
    public class ChnConstante : PersistenceData
    {
        [ColumnProperties("id_chnconstantes", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("blancoc_chnconstantes")]
        public decimal? BlancoC { get; set; }

        [ColumnProperties("blancoh_chnconstantes")]
        public decimal? BlancoH { get; set; }

        [ColumnProperties("blancon_chnconstantes")]
        public decimal? BlancoN { get; set; }

        [ColumnProperties("desvminc_chnconstantes")]
        public decimal? DesvMinC { get; set; }

        [ColumnProperties("desvminh_chnconstantes")]
        public decimal? DesvMinH { get; set; }

        [ColumnProperties("desvminn_chnconstantes")]
        public decimal? DesvMinN { get; set; }

        [ColumnProperties("desvmaxc_chnconstantes")]
        public decimal? DesvMaxC { get; set; }

        [ColumnProperties("desvmaxh_chnconstantes")]
        public decimal? DesvMaxH { get; set; }

        [ColumnProperties("desvmaxn_chnconstantes")]
        public decimal? DesvMaxN { get; set; }

        [ColumnProperties("idvprocedimiento_chnconstantes")]
        public int IdVProcedimiento { get; set; }
    }
}
