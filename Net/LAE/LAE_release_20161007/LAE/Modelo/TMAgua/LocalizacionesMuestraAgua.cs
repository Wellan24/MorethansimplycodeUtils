using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaLocalizacionesMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("localizaciones_muestraagua")]
    public class LocalizacionesMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_localizacionesmuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("descripcion_localizacionesmuestraagua")]
        public String Descripcion { get; set; }

        [ColumnProperties("idlocalizacion_localizacionesmuestraagua")]
        public int IdLocalizacion { get; set; }

        [ColumnProperties("idmuestraagua_localizacionesmuestraagua")]
        public int IdMuestra { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }
    }
}
