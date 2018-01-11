using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaFusibilidad
    {
        // TODO Rellenar esto con Selects necesarias.
    }


    [TableProperties("biomasa.replica_fusibilidad")]
    public class ReplicaFusibilidad : PersistenceData
    {
        [ColumnProperties("id_replicafusibilidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicafusibilidad")]
        public int Num { get; set; }

        [ColumnProperties("valor_replicafusibilidad")]
        public double? Valor { get; set; }

        [ColumnProperties("idudsvalor_replicafusibilidad")]
        public int? IdUdsValor { get; set; }

        [ColumnProperties("valido_replicafusibilidad")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idfusibilidad_replicafusibilidad")]
        public int IdFusibilidad { get; set; }
    }
}