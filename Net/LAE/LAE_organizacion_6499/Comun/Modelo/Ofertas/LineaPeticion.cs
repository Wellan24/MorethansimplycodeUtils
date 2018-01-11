using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo
{
    public class FactoriaLineas_peticion
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("lineas_peticion")]
    public class LineaPeticion : PersistenceData
    {
        [ColumnProperties("id_lineapeticion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("cantidad_lineapeticion")]
        public int Cantidad { get; set; }

        [ColumnProperties("idparametro_lineapeticion")]
        public int IdParametro { get; set; }

        [ColumnProperties("idpcontrolpeticion_lineapeticion")]
        public int IdPControlPeticion { get; set; }

        public override bool Equals(object obj)
        {
            LineaPeticion item = obj as LineaPeticion;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
