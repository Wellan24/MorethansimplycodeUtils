using Cartif.Logs;
using Dapper;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LAE.Comun.Modelo;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaCHN
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_CHN")]
    public class ReplicaChn : PersistenceData, IModelo, ITipoReplicas
    {
        [ColumnProperties("id_replicachn", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_replicachn")]
        public int Num { get; set; }

        [ColumnProperties("porcentajec_replicachn")]
        public double? PorcentajeC { get; set; }

        [ColumnProperties("idudsporcentajec_replicachn")]
        public int? IdUdsPorcentajeC { get; set; }

        [ColumnProperties("porcentajeh_replicachn")]
        public double? PorcentajeH { get; set; }

        [ColumnProperties("idudsporcentajeh_replicachn")]
        public int? IdUdsPorcentajeH { get; set; }

        [ColumnProperties("porcentajen_replicachn")]
        public double? PorcentajeN { get; set; }

        [ColumnProperties("idudsporcentajen_replicachn")]
        public int? IdUdsPorcentajeN { get; set; }

        [ColumnProperties("valido_replicachn")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idchn_replicachn")]
        public int IdCHN { get; set; }

        [ColumnProperties("idensayo_replicachn")]
        public int? IdEnsayo { get; set; }
    }
}