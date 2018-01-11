using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaLineas_peticion
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("lineas_peticion")]
    public class LineasPeticion : PersistenceData
    {
        [ColumnProperties("id_lineapeticion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("cantidad_lineapeticion")]
        public int Cantidad { get; set; }

        [ColumnProperties("idparametro_lineapeticion")]
        public int IdParametro { get; set; }

        [ColumnProperties("idpeticion_lineapeticion")]
        public int IdPeticion { get; set; }

        public override bool Equals(object obj)
        {
            LineasPeticion item = obj as LineasPeticion;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
