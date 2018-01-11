using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public class FactoriaTipomuestra_peticion
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("tipomuestra_peticion")]
    public class TipoMuestraPeticion : PersistenceData
    {
        [ColumnProperties("id_tipomuestrapeticion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idtipomuestra_tipomuestrapeticion")]
        public int IdTipoMuestra { get; set; }

        [ColumnProperties("idpeticion_tipomuestrapeticion")]
        public int IdPeticion { get; set; }

        public override bool Equals(object obj)
        {
            TipoMuestraPeticion item = obj as TipoMuestraPeticion;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
