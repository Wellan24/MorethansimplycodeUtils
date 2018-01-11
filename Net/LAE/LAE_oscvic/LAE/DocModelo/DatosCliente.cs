using Cartif.Collections;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocModelo
{
    class DatosCliente
    {
        public static void AddData(Cliente cl, CartifDictionary<string, string> lista)
        {
            lista.Add("nombrecliente", cl.Nombre);
            lista.Add("cifcliente", cl.Cif);
            lista.Add("direccioncliente", cl.Direccion);
            lista.Add("cpcliente", cl.CodigoPostal);
            lista.Add("localidadcliente", cl.Localidad);
            lista.Add("provinciacliente", cl.Provincia);
            lista.Add("paiscliente", cl.Pais);
            lista.Add("telefonocliente", cl.Telefono);
            lista.Add("faxcliente", cl.Fax);
            lista.Add("emailcliente", cl.Email);
        }
    }
}
