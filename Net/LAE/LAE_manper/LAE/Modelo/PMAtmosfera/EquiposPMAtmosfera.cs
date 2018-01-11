using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaEquipos_pmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("equipos_pmatmosfera")]
    public class EquiposPMAtmosfera : PersistenceData
    {
        [ColumnProperties("id_equipospmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("descripcion_equipospmatmosfera")]
        public String Descripcion { get; set; }

        [ColumnProperties("idequipo_equipospmatmosfera")]
        public int IdEquipo { get; set; }

        [ColumnProperties("idplanmedicion_equipospmatmosfera")]
        public int IdPlanMedicion { get; set; }
    }
}
