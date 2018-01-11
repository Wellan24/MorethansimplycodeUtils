using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaLineas_revisionoferta
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("lineas_revisionoferta")]
    public class LineasRevisionOferta : PersistenceData
    {
        [ColumnProperties("id_linearevisionoferta", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("cantidad_linearevisionoferta")]
        public int Cantidad { get; set; }

        [ColumnProperties("metodo_linearevisionoferta")]
        public String Metodo { get; set; }

        [ColumnProperties("idparametro_linearevisionoferta")]
        public int IdParametro { get; set; }

        [ColumnProperties("idrevisionoferta_linearevisionoferta")]
        public int IdRevisionOferta { get; set; }

        public override bool Equals(object obj)
        {
            LineasRevisionOferta item = obj as LineasRevisionOferta;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
