using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaClientes
    {
        public static Cliente[] RecuperarClientes()
        {
            return PersistenceManager.SelectAll<Cliente>().OrderBy(c => c.Nombre).ToArray();
        }
    }

    [TableProperties("clientes")]
    public class Cliente : PersistenceData, IModelo, IComparable<Cliente>
    {
        [ColumnProperties("id_cliente", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_cliente")]
        public String Nombre { get; set; }

        [ColumnProperties("direccion_cliente")]
        public String Direccion { get; set; }

        [ColumnProperties("telefonos_cliente")]
        public String Telefono { get; set; }

        [ColumnProperties("fax_cliente")]
        public String Fax { get; set; }

        [ColumnProperties("email_cliente")]
        public String Email { get; set; }

        [ColumnProperties("web_cliente")]
        public String Web { get; set; }

        [ColumnProperties("CIF_cliente")]
        public String Cif { get; set; }

        [ColumnProperties("codigopostal_cliente")]
        public String CodigoPostal { get; set; }

        [ColumnProperties("localidad_cliente")]
        public String Localidad { get; set; }

        [ColumnProperties("provincia_cliente")]
        public String Provincia { get; set; }

        [ColumnProperties("pais_cliente")]
        public String Pais { get; set; }

        public override bool Equals(object obj)
        {
            Cliente item = obj as Cliente;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }

        public override String ToString()
        {
            return Nombre;
        }

        public int CompareTo(Cliente other)
        {
            return this.Nombre.CompareTo(other.Nombre);
        }
    }
}
