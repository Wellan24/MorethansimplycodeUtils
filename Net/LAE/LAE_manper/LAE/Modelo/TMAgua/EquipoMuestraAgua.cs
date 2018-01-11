using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaEquipoMuestraAgua
    {
        private static int idEquipoConCodigo = 0;
        public static int GetIdEquipoConCodigo()
        {
            if (idEquipoConCodigo <= 0)
                idEquipoConCodigo = PersistenceManager.SelectByProperty<EquipoMuestraAgua>("Nombre", "F-TR-00-XX-00").FirstOrDefault()?.Id ?? 0;
            return idEquipoConCodigo;
        }

        private static int idOtrosSingleton = 0;
        public static int GetIdOtros()
        {
            if (idOtrosSingleton <= 0)
                idOtrosSingleton = PersistenceManager.SelectByProperty<EquipoMuestraAgua>("Nombre", "Otros").FirstOrDefault()?.Id ?? 0;
            return idOtrosSingleton;
        }

        private static EquipoMuestraAgua[] equipos = null;
        public static EquipoMuestraAgua[] GetEquipos()
        {
            if (equipos == null)
                equipos = PersistenceManager.SelectAll<EquipoMuestraAgua>().ToArray();
            return equipos;
        }
    }

    [TableProperties("equipo_muestraagua")]
    public class EquipoMuestraAgua : PersistenceData
    {
        [ColumnProperties("id_equipomuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_equipomuestraagua")]
        public String Nombre { get; set; }
    }
}
