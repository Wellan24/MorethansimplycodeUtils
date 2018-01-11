using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaFinos
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_finos")]
    public class ReplicaFinos : PersistenceData, IModelo
    {
        [ColumnProperties("id_replicafinos", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicafinos")]
        public int Num { get; set; }

        [ColumnProperties("m1_replicafinos")]
        public double? M1 { get; set; }

        [ColumnProperties("idudsm1_replicafinos")]
        public int? IdUdsM1 { get; set; }

        [ColumnProperties("m2_replicafinos")]
        public double? M2 { get; set; }

        [ColumnProperties("idudsm2_replicafinos")]
        public int? IdUdsM2 { get; set; }

        [ColumnProperties("valido_replicafinos")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idfinos_replicafinos")]
        public int IdFinos { get; set; }

        public double? Finos { get; set; }
    }
}