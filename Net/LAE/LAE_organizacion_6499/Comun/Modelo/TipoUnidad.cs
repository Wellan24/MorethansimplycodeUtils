using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public class FactoriaTipoUnidad
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tipo_unidad")]
    public class TipoUnidad : PersistenceData
    {
        [ColumnProperties("id_tipounidad")]
        public int Id { get; set; }

        [ColumnProperties("nombre_tipounidad")]
        public String Nombre { get; set; }
    }
}
