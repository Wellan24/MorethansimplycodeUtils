using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaFrealizada_fpmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("frealizada_fpmatmosfera")]
    public class FechaRealizadaFocoAtm : PersistenceData
    {
        [ColumnProperties("id_frealizadafpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fecha_frealizadafpmatmosfera")]
        public DateTime Fecha { get; set; }

        [ColumnProperties("idfoco_frealizadafpmatmosfera")]
        public int IdFoco { get; set; }
    }
}
