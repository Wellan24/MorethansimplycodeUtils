using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaContactos
    {
        // TODO Rellenar esto con Selects necesarias.
    }
    
    [TableProperties("contactos")]
    public class Contacto : PersistenceData, IModelo
    {
        [ColumnProperties("id_contacto", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_contacto")]
        public String Nombre { get; set; }

        [ColumnProperties("apellidos_contacto")]
        public String Apellidos { get; set; }

        [ColumnProperties("telefono_contacto")]
        public int? Telefono { get; set; }

        [ColumnProperties("email_contacto")]
        public String Email { get; set; }

        [ColumnProperties("idcliente_contacto")]
        public int IdCliente { get; set; }

        [ColumnProperties("idtecnico_contacto")]
        public int IdTecnico { get; set; }

        public override bool Equals(object obj)
        {
            Contacto item = obj as Contacto;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }

        public override String ToString()
        {
            return Nombre + " " + Apellidos;
        }
    }
}
