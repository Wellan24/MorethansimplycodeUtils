using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaDurabilidad
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_durabilidad")]
    public class ReplicaDurabilidad : PersistenceData, IModelo
    {
        [ColumnProperties("id_replicadurabilidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicadurabilidad")]
        public int Num { get; set; }

        [ColumnProperties("m2_replicadurabilidad")]
        public double? M2 { get; set; }

        [ColumnProperties("idudsm2_replicadurabilidad")]
        public int? IdUdsM2 { get; set; }

        [ColumnProperties("m3_replicadurabilidad")]
        public double? M3 { get; set; }

        [ColumnProperties("idudsm3_replicadurabilidad")]
        public int? IdUdsM3 { get; set; }

        [ColumnProperties("valido_replicadurabilidad")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("iddurabilidad_replicadurabilidad")]
        public int IdDurabilidad { get; set; }
        
        public double? Durabilidad { get; set; }
    }
}