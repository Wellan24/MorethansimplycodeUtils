using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMedicion
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("particulas.medicion")]
    public class Medicion : PersistenceData
    {
        [ColumnProperties("id_medicion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fechaentradaacondicionamiento_medicion")]
        public DateTime FechaEntradaAcondicionamiento { get; set; }

        [ColumnProperties("fechasalidaacondicionamiento_medicion")]
        public DateTime FechaSalidaAcondicionamiento { get; set; }

        [ColumnProperties("fecha_medicion")]
        public DateTime Fecha { get; set; }

        [ColumnProperties("presion_medicion")]
        public int? Presion { get; set; }

        [ColumnProperties("temperatura_medicion")]
        public int? Temperatura { get; set; }

        [ColumnProperties("humedad_medicion")]
        public int? Humedad { get; set; }

        [ColumnProperties("idtecnico_medicion")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idsoporte_medicion")]
        public int? IdSoporte { get; set; }

        public ObservableCollection<Pesada> Pesadas;
    }
}