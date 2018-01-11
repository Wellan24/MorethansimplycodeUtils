using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMuestras
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("muestras")]
    public class Muestra : PersistenceData
    {
        [ColumnProperties("id_muestra", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("codigo_muestra")]
        public String Codigo { get; set; }

        [ColumnProperties("idsolicitud_muestra")]
        public int IdSolicitud { get; set; }

        [ColumnProperties("idtecnico_muestra")]
        public int IdTecnico { get; set; }
    }
}
