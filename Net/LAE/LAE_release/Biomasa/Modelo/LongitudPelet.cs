using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaLongitudesPelet
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.longitudes_pelet")]
    public class LongitudPelet : PersistenceData, IModelo
    {
        [ColumnProperties("id_longitudespelet", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("medida_longitudespelet")]
        public double? Medida { get; set; }

        [ColumnProperties("idudsmedida_longitudespelet")]
        public int IdUdsMedida { get; set; }

        [ColumnProperties("idclase_longitudespelet")]
        public int IdClase { get; set; }

        public bool Selected { get; set; }
    }
}