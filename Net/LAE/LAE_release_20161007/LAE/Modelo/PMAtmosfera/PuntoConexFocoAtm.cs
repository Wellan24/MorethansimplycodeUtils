using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaPunto_confpmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("punto_confpmatmosfera")]
    public class PuntoConexFocoAtmosfera : PersistenceData
    {
        [ColumnProperties("id_puntoconfpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("loc_puntoconfpmatmosfera")]
        public decimal? Localizacion { get; set; }

        [ColumnProperties("velocidad_puntoconfpmatmosfera")]
        public decimal? Velocidad { get; set; }

        [ColumnProperties("idudsvelocidad_puntoconfpmatmosfera")]
        public int IdUdsVelocidad { get; set; }

        [ColumnProperties("temperatura_puntoconfpmatmosfera")]
        public decimal? Temperatura { get; set; }

        [ColumnProperties("idudstemperatura_puntoconfpmatmosfera")]
        public int IdUdsTemperatura { get; set; }

        [ColumnProperties("angulo_puntoconfpmatmosfera")]
        public Boolean AnguloEje { get; set; }

        [ColumnProperties("idconexion_puntoconfpmatmosfera")]
        public int IdConexion { get; set; }
    }
}
