using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaEquiposMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("equipos_muestraagua")]
    public class EquiposMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_equiposmuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("codigo_equiposmuestraagua")]
        public String Codigo { get; set; }

        [ColumnProperties("descripcion_equiposmuestraagua")]
        public String Descripcion { get; set; }

        [ColumnProperties("idequipo_equiposmuestraagua")]
        public int IdEquipo { get; set; }

        [ColumnProperties("idmuestraagua_equiposmuestraagua")]
        public int IdMuestra { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }
    }
}
