using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMaterialMuestraAgua
    {
        private static int idOtrosSingleton = 0;
        public static int GetIdOtros()
        {
            if (idOtrosSingleton <= 0)
                idOtrosSingleton = PersistenceManager.SelectByProperty<MaterialMuestraAgua>("Nombre", "Otros").FirstOrDefault()?.Id ?? 0;

            return idOtrosSingleton;
        }

        private static MaterialMuestraAgua[] materiales = null;
        public static MaterialMuestraAgua[] GetTiposMaterial()
        {
            if (materiales == null)
                materiales = PersistenceManager.SelectAll<MaterialMuestraAgua>().ToArray();
            return materiales;
        }
    }

    [TableProperties("material_muestraagua")]
    public class MaterialMuestraAgua : PersistenceData
    {
        [ColumnProperties("id_materialmuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_materialmuestraagua")]
        public String Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
