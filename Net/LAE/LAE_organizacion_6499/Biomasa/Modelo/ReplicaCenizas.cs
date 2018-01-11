using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaCenizas
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_cenizas")]
    public class ReplicaCeniza : PersistenceData, IModelo, ITipoReplicas
    {
        [ColumnProperties("id_replicacenizas", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicacenizas")]
        public int Num { get; set; }

        [ColumnProperties("m1_rreplicacenizas")]
        public double? M1 { get; set; }

        [ColumnProperties("idudsm1_replicacenizas")]
        public int? IdUdsM1 { get; set; }

        [ColumnProperties("m2_replicacenizas")]
        public double? M2 { get; set; }

        [ColumnProperties("idudsm2_replicacenizas")]
        public int? IdUdsM2 { get; set; }

        [ColumnProperties("m3_replicacenizas")]
        public double? M3 { get; set; }

        [ColumnProperties("idudsm3_replicacenizas")]
        public int? IdUdsM3 { get; set; }

        [ColumnProperties("valido_replicacenizas")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idcenizas_replicacenizas")]
        public int IdCenizas { get; set; }

        public double? Cenizas { get; set; }
    }
}
