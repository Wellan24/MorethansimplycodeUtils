using Cartif.Collections;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocModelo
{
    class DatosContacto
    {
        public static void AddData(Contacto c, CartifDictionary<string, string> lista)
        {
            lista.Add("nombrecontacto", c.Nombre+" "+c.Apellidos);
            lista.Add("emailcontacto", c.Email);
        }
    }
}
