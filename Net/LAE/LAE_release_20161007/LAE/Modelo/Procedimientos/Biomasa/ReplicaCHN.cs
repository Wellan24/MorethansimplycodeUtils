using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaCHN
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_CHN")]
    public class ReplicaCHN : PersistenceData
    {
        [ColumnProperties("id_replicachn", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicachn")]
        public int Num { get; set; }

        [ColumnProperties("porcentaje_replicachn")]
        public double? Porcentaje { get; set; }

        [ColumnProperties("idudsporcentaje_replicachn")]
        public int? IdUdsPorcentaje { get; set; }

        [ColumnProperties("valido_replicachn")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idchn_replicachn")]
        public int IdCHN { get; set; }
    }
}
