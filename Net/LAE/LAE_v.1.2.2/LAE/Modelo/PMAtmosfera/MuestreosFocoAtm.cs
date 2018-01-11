using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMuestreos_fpmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("muestreos_fpmatmosfera")]
    public class MuestreosFocoAtm : PersistenceData
    {
        [ColumnProperties("id_muestreosfpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idmuestreo_muestreosfpmatmosfera")]
        public int IdMuestreo { get; set; }

        [ColumnProperties("idfoco_muestreosfpmatmosfera")]
        public int IdFoco { get; set; }
    }
}
