using Cartif.Logs;
using Dapper;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Comun.Modelo
{
    public class FactoriaLineas_revisionoferta
    {

    }

    [TableProperties("lineas_revisionoferta")]
    public class LineaRevisionOferta : PersistenceData
    {
        [ColumnProperties("id_linearevisionoferta", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("cantidad_linearevisionoferta")]
        public int Cantidad { get; set; }

        [ColumnProperties("idparametro_linearevisionoferta")]
        public int IdParametro { get; set; }

        [ColumnProperties("idpcontrolrevision_linearevisionoferta")]
        public int IdPControlRevisionOferta { get; set; }

        public override bool Equals(object obj)
        {
            LineaRevisionOferta item = obj as LineaRevisionOferta;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
