using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaDensidad
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_densidad")]
    public class ReplicaDensidad : PersistenceData, IModelo, ITipoReplicas
    {
        [ColumnProperties("id_replicadensidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicadensidad")]
        public int Num { get; set; }

        [ColumnProperties("m1_replicadensidad")]
        public double? M1 { get; set; }

        [ColumnProperties("idudsm1_replicadensidad")]
        public int? IdUdsM1 { get; set; }

        [ColumnProperties("m2_replicadensidad")]
        public double? M2 { get; set; }

        [ColumnProperties("idudsm2_replicadensidad")]
        public int? IdUdsM2 { get; set; }

        [ColumnProperties("valido_replicadensidad")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("iddensidad_replicadensidad")]
        public int IdDensidad { get; set; }

        public double? Densidad { get; set; }
    }
}