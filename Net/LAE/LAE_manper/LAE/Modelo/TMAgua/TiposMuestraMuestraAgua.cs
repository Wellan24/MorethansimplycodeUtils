using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaTiposMuestraMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tiposmuestra_muestraagua")]
    public class TiposMuestraMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_tiposmuestramuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("horas_tiposmuestramuestraagua")]
        public decimal? Horas { get; set; }

        [ColumnProperties("idudshoras_tiposmuestramuestraagua")]
        public int? IdUdsHoras { get; set; }

        [ColumnProperties("numporciones_muestratipomuestreo")]
        public int? NumPorciones { get; set; }

        [ColumnProperties("intervalo_tiposmuestramuestraagua")]
        public int? Intervalo { get; set; }

        [ColumnProperties("idudsintervalo_tiposmuestramuestraagua")]
        public int? IdUdsIntervalo { get; set; }

        [ColumnProperties("volumen_tiposmuestramuestraagua")]
        public decimal? Volumen { get; set; }

        [ColumnProperties("idudsvolumen_tiposmuestramuestraagua")]
        public int? IdUdsVolumen { get; set; }

        [ColumnProperties("idtipomuestra_tiposmuestramuestraagua")]
        public int IdTipoMuestra { get; set; }

        [ColumnProperties("idmuestraagua_tiposmuestramuestraagua")]
        public int IdMuestra { get; set; }
    }
}
