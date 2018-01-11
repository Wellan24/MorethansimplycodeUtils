using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaFusibilidad
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_fusibilidad")]
    public class ReplicaFusibilidad : PersistenceData, IModelo, ITipoReplicas
    {
        [ColumnProperties("id_replicafusibilidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicafusibilidad")]
        public int Num { get; set; }

        [ColumnProperties("temperaturasst_replicafusibilidad")]
        public int? TemperaturaSst { get; set; }

        [ColumnProperties("idudstemperaturasst_replicafusibilidad")]
        public int? IdUdsTemperaturaSst { get; set; }

        [ColumnProperties("temperaturadt_replicafusibilidad")]
        public int? TemperaturaDT { get; set; }

        [ColumnProperties("idudstemperaturadt_replicafusibilidad")]
        public int? IdUdsTemperaturaDT { get; set; }

        [ColumnProperties("temperaturaht_replicafusibilidad")]
        public int? TemperaturaHT { get; set; }

        [ColumnProperties("mayorht_replicafusibilidad")]
        public Boolean MayorHT { get; set; }

        [ColumnProperties("idudstemperaturaht_replicafusibilidad")]
        public int? IdUdsTemperaturaHT { get; set; }

        [ColumnProperties("temperaturaft_replicafusibilidad")]
        public int? TemperaturaFT { get; set; }

        [ColumnProperties("mayorfT_replicafusibilidad")]
        public Boolean MayorFT { get; set; }

        [ColumnProperties("idudstemperaturaft_replicafusibilidad")]
        public int? IdUdsTemperaturaFT { get; set; }

        [ColumnProperties("valido_replicafusibilidad")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idfusibilidad_replicafusibilidad")]
        public int IdFusibilidad { get; set; }

        [ColumnProperties("idensayo_replicafusibilidad")]
        public int? IdEnsayo { get; set; }
    }
}