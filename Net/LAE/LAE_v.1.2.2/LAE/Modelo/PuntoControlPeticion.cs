using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{

    public static class FactoriaPuntocontrolPeticion
    {
        public static void LoadLineasPeticion(this PuntocontrolPeticion pc)
        {
            pc.LineasPeticion = PersistenceManager.SelectByProperty<LineaPeticion>("IdPControlPeticion", pc.Id).ToArray();
        }
    }

    [TableProperties("puntocontrol_peticion")]
    public class PuntocontrolPeticion : PersistenceData
    {
        [ColumnProperties("id_puntocontrolpeticion", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_puntocontrolpeticion")]
        public String Nombre { get; set; }

        [ColumnProperties("observaciones_puntocontrolpeticion")]
        public String Observaciones { get; set; }

        [ColumnProperties("importe_puntocontrolpeticion")]
        public decimal Importe { get; set; }

        [ColumnProperties("idpeticion_puntocontrolpeticion")]
        public int IdPeticion { get; set; }

        public LineaPeticion[] LineasPeticion { get; set; }

        public override bool Equals(object obj)
        {
            PuntocontrolPeticion item = obj as PuntocontrolPeticion;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }

}
