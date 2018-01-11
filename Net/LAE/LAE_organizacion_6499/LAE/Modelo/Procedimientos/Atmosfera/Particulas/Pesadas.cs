using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaPesadas
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("particulas.pesadas")]
    public class Pesada : PersistenceData
    {
        [ColumnProperties("id_pesada", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("tiempo_pesada")]
        public int Tiempo { get; set; }

        [ColumnProperties("idudstiempo_pesada")]
        public int? IdUdsTiempo { get; set; }

        [ColumnProperties("valor_pesada")]
        public double Valor { get; set; }

        [ColumnProperties("idudsvalor_pesada")]
        public int? IdUdsValor { get; set; }

        [ColumnProperties("idmedicion_pesada")]
        public int? IdMedicion { get; set; }
    }
}