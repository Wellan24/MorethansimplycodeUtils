using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaEquipoPMAtmosfera
    {
        private static int idOtrosSingleton = 0;
        public static int GetIdOtros()
        {
            if (idOtrosSingleton <= 0)
                idOtrosSingleton = PersistenceManager.SelectByProperty<EquipoPMAtmosfera>("Nombre", "Otros").FirstOrDefault()?.Id ?? 0;
            return idOtrosSingleton;
        }

        private static EquipoPMAtmosfera[] equipos = null;
        public static EquipoPMAtmosfera[] GetEquipos()
        {
            if (equipos == null)
                equipos = PersistenceManager.SelectAll<EquipoPMAtmosfera>().ToArray();
            return equipos;
        }
    }

    [TableProperties("equipo_pmatmosfera")]
    public class EquipoPMAtmosfera : PersistenceData
    {
        [ColumnProperties("id_equipopmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_equipopmatmosfera")]
        public String Nombre { get; set; }
    }

}
