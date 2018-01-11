using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public class FactoriaTipomuestraRevision
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tipomuestra_revision")]
    public class TipoMuestraRevision : PersistenceData
    {
        [ColumnProperties("id_tipomuestrarevision", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idtipomuestra_tipomuestrarevision")]
        public int IdTipoMuestra { get; set; }

        [ColumnProperties("idrevision_tipomuestrarevision")]
        public int IdRevision { get; set; }

        public override bool Equals(object obj)
        {
            TipoMuestraRevision item = obj as TipoMuestraRevision;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
