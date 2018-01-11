using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaSCl
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_SCl")]
    public class ReplicaSCl : PersistenceData
    {
        [ColumnProperties("id_replicascl", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicascl")]
        public int Num { get; set; }

        [ColumnProperties("m_replicaSCl")]
        public double? M { get; set; }

        [ColumnProperties("idudsm_replicaSCl")]
        public int? IdUdsM { get; set; }

        [ColumnProperties("muestra_replicascl")]
        public double? Muestra { get; set; }

        [ColumnProperties("idudsmuestra_replicascl")]
        public int? IdUdsMuestra { get; set; }

        [ColumnProperties("valido_replicascl")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idscl_replicascl")]
        public int IdSCl { get; set; }

        public double? Porcentaje { get; set; }
    }
}