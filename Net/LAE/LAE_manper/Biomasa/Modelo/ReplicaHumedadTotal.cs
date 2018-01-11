using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaHumedadTotal
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_humedadtotal")]
    public class ReplicaHumedadTotal : PersistenceData, IModelo, ITipoReplicas
    {
        [ColumnProperties("id_replicahumedadtotal", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicahumedadtotal")]
        public int Num { get; set; }

        [ColumnProperties("m1_replicahumedadtotal")]
        public double? M1 { get; set; }

        [ColumnProperties("idudsm1_replicahumedadtotal")]
        public int? IdUdsM1 { get; set; }

        [ColumnProperties("m2_replicahumedadtotal")]
        public double? M2 { get; set; }

        [ColumnProperties("idudsm2_replicahumedadtotal")]
        public int? IdUdsM2 { get; set; }

        [ColumnProperties("m3_replicahumedadtotal")]
        public double? M3 { get; set; }

        [ColumnProperties("idudsm3_replicahumedadtotal")]
        public int? IdUdsM3 { get; set; }

        [ColumnProperties("valido_replicahumedadtotal")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idhumedad_replicahumedadtotal")]
        public int IdHumedad { get; set; }

        public double? HumedadTotal { get; set; }
    }
}