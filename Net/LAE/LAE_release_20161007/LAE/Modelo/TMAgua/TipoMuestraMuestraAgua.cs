using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaTipoMuestraMuestraAgua
    {
        private static int idPuntualSingleton = 0;
        public static int GetIdPuntual()
        {
            if (idPuntualSingleton <= 0)
                idPuntualSingleton = PersistenceManager.SelectByProperty<TipoMuestraMuestraAgua>("Nombre", "Puntual").FirstOrDefault()?.Id ?? 0;
            return idPuntualSingleton;
        }

        private static TipoMuestraMuestraAgua[] tiposMuestra = null;
        public static TipoMuestraMuestraAgua[] GetTiposMuestra()
        {
            if (tiposMuestra == null)
                tiposMuestra = PersistenceManager.SelectAll<TipoMuestraMuestraAgua>().ToArray();
            return tiposMuestra;
        }
    }

    [TableProperties("tipomuestra_muestraagua")]
    public class TipoMuestraMuestraAgua : PersistenceData
    {
        [ColumnProperties("id_tipomuestramuetraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_tipomuestramuetraagua")]
        public String Nombre { get; set; }

        [ColumnProperties("compuesta_tipomuestramuetraagua")]
        public Boolean Compuesta { get; set; }
    }
}
