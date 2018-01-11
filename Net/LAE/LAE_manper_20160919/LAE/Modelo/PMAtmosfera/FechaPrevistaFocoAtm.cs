using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaFprevista_fpmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("fprevista_fpmatmosfera")]
    public class FechaPrevistaFocoAtm : PersistenceData
    {
        [ColumnProperties("id_fprevistafpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fecha_fprevistafpmatmosfera")]
        public DateTime Fecha { get; set; }

        [ColumnProperties("idfoco_fprevistafpmatmosfera")]
        public int IdFoco { get; set; }
    }
}
