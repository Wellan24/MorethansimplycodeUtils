using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaReplicaChncontrol
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_chncontrol")]
    public class ReplicaCHNcontrol : PersistenceData
    {
        [ColumnProperties("id_replicachncontrol", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicachncontrol")]
        public int Num { get; set; }

        [ColumnProperties("ordenensayo_replicachncontrol")]
        public int OrdenEnsayo { get; set; }

        [ColumnProperties("porcentajec_replicachncontrol")]
        public double? PorcentajeC { get; set; }

        [ColumnProperties("idudsporcentajec_replicachncontrol")]
        public int? IdUdsPorcentajeC { get; set; }

        [ColumnProperties("porcentajeh_replicachncontrol")]
        public double? PorcentajeH { get; set; }

        [ColumnProperties("idudsporcentajeh_replicachncontrol")]
        public int? IdUdsPorcentajeH { get; set; }

        [ColumnProperties("porcentajen_replicachncontrol")]
        public double? PorcentajeN { get; set; }

        [ColumnProperties("idudsporcentajen_replicachncontrol")]
        public int? IdUdsPorcentajeN { get; set; }

        [ColumnProperties("valido_replicachncontrol")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idchn_replicachncontrol")]
        public int IdCHN { get; set; }

        [ColumnProperties("idensayo_replicachncontrol")]
        public int IdEnsayo { get; set; }
    }
}