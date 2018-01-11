using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaFusibilidadcontrol
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_fusibilidadcontrol")]
    public class ReplicaFusibilidadControl : PersistenceData, IModelo
    {
        [ColumnProperties("id_replicafusibilidadcontrol", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("temperatura_replicafusibilidadcontrol")]
        public int? Temperatura { get; set; }

        [ColumnProperties("idudstemperatura_replicafusibilidadcontrol")]
        public int? IdUdsTemperatura { get; set; }

        [ColumnProperties("idfusibilidad_replicafusibilidadcontrol")]
        public int IdFusibilidad { get; set; }
    }
}