using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaEquipoMedicion
    {
        // TODO Rellenar esto con Selects necesarias.
        public static EquipoMedicion[] GetEquipos(MedicionPNT medicion)
        {
            return PersistenceManager.SelectByProperty<EquipoMedicion>("IdMedicion", medicion.Id).ToArray();
        }
    }

    [TableProperties("equipo_medicion")]
    public class EquipoMedicion : PersistenceData, IModelo
    {
        [ColumnProperties("id_equipomedicion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idequipo_equipomedicion")]
        public int IdEquipo { get; set; }

        [ColumnProperties("idmedicion_equipomedicion")]
        public int IdMedicion { get; set; }
    }
}