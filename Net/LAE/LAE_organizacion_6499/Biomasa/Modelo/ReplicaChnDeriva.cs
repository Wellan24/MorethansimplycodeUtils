using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaReplicaChnderiva
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.replica_chnderiva")]
    public class ReplicaChnDeriva : PersistenceData, IModelo
    {
        [ColumnProperties("id_replicachnderiva", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("masac_replicachnderiva")]
        public decimal? MasaC { get; set; }

        [ColumnProperties("idudsmasac_replicachnderiva")]
        public int? IdUdsMasaC { get; set; }

        [ColumnProperties("masah_replicachnderiva")]
        public decimal? MasaH { get; set; }

        [ColumnProperties("idudsmasah_replicachnderiva")]
        public int? IdUdsMasaH { get; set; }

        [ColumnProperties("masan_replicachnderiva")]
        public decimal? MasaN { get; set; }

        [ColumnProperties("idudsmasan_replicachnderiva")]
        public int? IdUdsMasaN { get; set; }

        [ColumnProperties("valorc_replicachnderiva")]
        public decimal? ValorC { get; set; }

        [ColumnProperties("idudsvalorc_replicachnderiva")]
        public int? IdUdsValorC { get; set; }

        [ColumnProperties("valorh_replicachnderiva")]
        public decimal? ValorH { get; set; }

        [ColumnProperties("idudsvalorh_replicachnderiva")]
        public int? IdUdsValorH { get; set; }

        [ColumnProperties("valorn_replicachnderiva")]
        public decimal? ValorN { get; set; }

        [ColumnProperties("idudsvalorn_replicachnderiva")]
        public int? IdUdsValorN { get; set; }

        [ColumnProperties("valido_replicachnderiva")]
        public Boolean? Valido { get; set; }

        [ColumnProperties("idchnderiva_replicachnderiva")]
        public int IdCHNderiva { get; set; }
    }
}
