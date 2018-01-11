using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaHumedad3
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_humedad3")]
    public class ReplicaHumedad3 : PersistenceData, IModelo, ITipoReplicas
    {
        [ColumnProperties("id_replicahumedad3", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicahumedad3")]
        public int Num { get; set; }

        [ColumnProperties("m1_replicahumedad3")]
        public double? M1 { get; set; }

        [ColumnProperties("idudsm1_replicahumedad3")]
        public int? IdUdsM1 { get; set; }

        [ColumnProperties("m2_replicahumedad3")]
        public double? M2 { get; set; }

        [ColumnProperties("idudsm2_replicahumedad3")]
        public int? IdUdsM2 { get; set; }

        [ColumnProperties("m3_replicahumedad3")]
        public double? M3 { get; set; }

        [ColumnProperties("idudsm3_replicahumedad3")]
        public int? IdUdsM3 { get; set; }

        [ColumnProperties("valido_replicahumedad3")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idhumedad_replicahumedad3")]
        public int IdHumedad { get; set; }

        public double? HumedadTotal { get; set; }
    }
}