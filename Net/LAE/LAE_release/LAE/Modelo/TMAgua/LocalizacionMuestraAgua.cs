using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaLocalizacionMuestraAgua
    {
        private static int idOtrosSingleton = 0;
        public static int GetIdOtros()
        {
            if (idOtrosSingleton <= 0)
                idOtrosSingleton = PersistenceManager.SelectByProperty<LocalizacionMuestraAgua>("Nombre", "Otros").FirstOrDefault()?.Id ?? 0;

            return idOtrosSingleton;
        }

        private static LocalizacionMuestraAgua[] materiales = null;
        public static LocalizacionMuestraAgua[] GetTiposLocalizacion()
        {
            if (materiales == null)
                materiales = PersistenceManager.SelectAll<LocalizacionMuestraAgua>().ToArray();
            return materiales;
        }
    }

    [TableProperties("localizacion_muestraagua")]
    public class LocalizacionMuestraAgua : PersistenceData
    {
        [ColumnProperties("id_localizacionmuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_localizacionmuestraagua")]
        public String Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
