using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public class FactoriaTipoTecnico
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tipo_tecnico")]
    public class TipoTecnico : PersistenceData
    {
        [ColumnProperties("id_tipotecnico", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idtecnico_tipotecnico")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idtipo_tipotecnico")]
        public int IdTipo { get; set; }
    }
}