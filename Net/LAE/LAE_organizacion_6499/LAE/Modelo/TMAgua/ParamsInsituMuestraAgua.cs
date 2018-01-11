using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaParamsInsituMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("paramsinsitu_muestraagua")]
    public class ParamsInsituMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_paramsinsitumuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("temperatura_paramsinsitumuestraagua")]
        public decimal? Temperatura { get; set; }

        [ColumnProperties("idudstemperatura_paramsinsitumuestraagua")]
        public int? IdUdsTemperatura { get; set; }

        [ColumnProperties("caudal_paramsinsitumuestraagua")]
        public decimal? Caudal { get; set; }

        [ColumnProperties("idudscaudal_paramsinsitumuestraagua")]
        public int? IdUdsCaudal { get; set; }

        [ColumnProperties("caudalimetrolae_paramsinsitumuestraagua")]
        public Boolean? CaudalimetroLAE { get; set; }

        [ColumnProperties("idparaminsitu_paramsinsitumuestraagua")]
        public int IdParamsInsitu { get; set; }

        [ColumnProperties("idmuestraagua_paramsinsitumuestraagua")]
        public int IdMuestra { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }
    }
}
