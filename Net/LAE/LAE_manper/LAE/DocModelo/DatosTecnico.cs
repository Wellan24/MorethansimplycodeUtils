using Cartif.Collections;
using LAE.Comun.Modelo;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocModelo
{
    class DatosTecnico
    {
        public static void AddData(Tecnico t, CartifDictionary<string, string> lista)
        {
            lista.Add("nombretecnico", t.Nombre + " " + t.PrimerApellido + " " + t.SegundoApellido);
        }
    }
}
