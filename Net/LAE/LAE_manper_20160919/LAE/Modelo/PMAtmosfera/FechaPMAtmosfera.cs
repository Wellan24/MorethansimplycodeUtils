using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaFecha_pmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("fecha_pmatmosfera")]
    public class FechaPMAtmosfera : PersistenceData
    {
        [ColumnProperties("id_fechapmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fechaprevista_fechapmatmosfera")]
        public DateTime? FechaPrevista { get; set; }

        [ColumnProperties("idplanmedicion_fechapmatmosfera")]
        public int IdPlanMedicion { get; set; }
    }
}
