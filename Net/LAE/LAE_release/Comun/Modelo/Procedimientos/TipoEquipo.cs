using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaTipoEquipo
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tipo_equipo")]
    public class TipoEquipo : PersistenceData
    {
        [ColumnProperties("id_tipoequipo", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_tipoequipo")]
        public String Nombre { get; set; }
    }

    
}