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

    }

    [TableProperties("clientes")]
    public class Cliente : PersistenceData, IModelo
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

    }
}
