using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMaterialesMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("materiales_muestraagua")]
    public class MaterialesMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_materialesmuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("descripcion_materialesmuestraagua")]
        public String Descripcion { get; set; }

        [ColumnProperties("idmaterial_materialesmuestraagua")]
        public int IdMaterial { get; set; }

        [ColumnProperties("idmuestraagua_materialesmuestraagua")]
        public int IdMuestra { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }
    }
}
