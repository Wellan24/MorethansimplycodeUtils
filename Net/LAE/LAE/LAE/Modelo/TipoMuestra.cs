using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaTipos_muestra
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tipos_muestra")]
    public class TipoMuestra : PersistenceData
    {
        [ColumnProperties("id_tipomuestra", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_tipomuestra")]
        public String Nombre { get; set; }

        public override String ToString()
        {
            return Nombre;
        }
    }
}
