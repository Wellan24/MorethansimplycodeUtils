using Cartif.Collections;
using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
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
    public class Contacto : PersistenceData, IModelo, IComparable<Contacto>
    {
        [ColumnProperties("id_contacto", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_contacto")]
        public String Nombre { get; set; }

        [ColumnProperties("apellidos_contacto")]
        public String Apellidos { get; set; }

        [ColumnProperties("telefono_contacto")]
        public String Telefono { get; set; }

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
            return Nombre + " " + Apellidos + " (" + Email + ")";
        }

        public int CompareTo(Contacto other)
        {
            int comparaNombre = this.Nombre.CompareTo(other.Nombre);
            if (comparaNombre != 0)
                return comparaNombre;
            else
            {
                if (this.Apellidos != null)
                {
                    int comparaApellido = this.Apellidos.CompareTo(other.Apellidos);
                    if (comparaApellido != 0)
                        return comparaApellido;
                }
            }
            return this.Email.CompareTo(other.Email);
        }
    }
}
