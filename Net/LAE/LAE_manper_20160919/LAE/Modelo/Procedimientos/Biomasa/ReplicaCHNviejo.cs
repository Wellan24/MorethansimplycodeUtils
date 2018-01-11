using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaCHNviejo
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_CHN")]
    public class ReplicaCHNviejo : PersistenceData
    {
        [ColumnProperties("id_replicachn", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicachn")]
        public int Num { get; set; }

        [ColumnProperties("ordenensayo_replicachn")]
        public int OrdenEnsayo { get; set; }

        [ColumnProperties("porcentaje_replicachn")]
        public double? Porcentaje { get; set; }

        [ColumnProperties("idudsporcentaje_replicachn")]
        public int? IdUdsPorcentaje { get; set; }

        [ColumnProperties("valido_replicachn")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idchn_replicachn")]
        public int IdCHN { get; set; }

        [ColumnProperties("idensayo_replicachn")]
        public int IdEnsayo { get; set; }
    }
}
