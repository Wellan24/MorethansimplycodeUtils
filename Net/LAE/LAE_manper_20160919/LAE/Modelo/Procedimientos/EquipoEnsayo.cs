using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaEquipoEnsayo
    {
        // TODO Rellenar esto con Selects necesarias.
        public static EquipoEnsayo[] GetEquipos(EnsayoPNT ensayo)
        {
            return PersistenceManager.SelectByProperty<EquipoEnsayo>("IdEnsayo", ensayo.Id).ToArray();
        }
    }

    [TableProperties("equipo_ensayo")]
    public class EquipoEnsayo : PersistenceData
    {
        [ColumnProperties("id_equipoensayo", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idequipo_equipoensayo")]
        public int IdEquipo { get; set; }

        [ColumnProperties("idensayo_equipoensayo")]
        public int IdEnsayo { get; set; }
    }
}