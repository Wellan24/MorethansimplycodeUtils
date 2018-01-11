using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaPersonal_pmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("personal_pmatmosfera")]
    public class PersonalPMAtmosfera : PersistenceData
    {
        [ColumnProperties("id_personalpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idtecnico_personalpmatmosfera")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idplanmedicion_personalpmatmosfera")]
        public int IdPlanMedicion { get; set; }
    }
}
