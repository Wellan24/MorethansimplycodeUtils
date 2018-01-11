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
    class DatosPeticion
    {

        public static void AddData(Peticion p, CartifDictionary<string, string> lista)
        {
            lista.Add("lugarpeticion", (p.RequiereTomaMuestra == true) ? p.LugarMuestra : "");
            lista.Add("puntospeticion", (p.RequiereTomaMuestra == true) ? p.NumPuntosMuestreo.ToString() : "");

            List<TipoMuestra> tipos = FactoriaTipoMuestra.GetMuestrasPeticion(p).ToList();
            String tiposM = "";
            tipos.ForEach(
                t =>
                {
                    if (t.Nombre != "Aguas" && t.Nombre != "Atmósfera")
                        tiposM += t.Nombre + ", ";
                });
            if (!tiposM.Equals(""))
                lista.Add("otrostipos", tiposM.Remove(tiposM.Length - 2));

            lista.Add("frecuencia", (p.TrabajoPuntual == true) ? "" : p.Frecuencia);
            lista.Add("plazo", p.PlazoRealizacion);
            lista.Add("observacionespeticion", p.Observaciones);
            lista.Add("fechapeticion", (p.Fecha == null) ? "" : (p.Fecha ?? DateTime.Now).ToString("dd 'de' MMMM 'de' yyyy"));
        }

        public static void AddListaCheck(Peticion p, List<string> lista)
        {
            lista.Add((p.RequiereTomaMuestra == true) ? "cartif" : "cliente");


            List<TipoMuestra> tipos = FactoriaTipoMuestra.GetMuestrasPeticion(p).ToList();
            TipoMuestra tipo;
            tipo = tipos.Find(t => t.Nombre.Equals("Aguas"));
            if (tipo != null)
            {
                lista.Add("aguas");
                tipos.Remove(tipo);
            }
            tipo = tipos.Find(t => t.Nombre.Equals("Atmósfera"));
            if (tipo != null)
            {
                lista.Add("atmosfera");
                tipos.Remove(tipo);
            }
            if (tipos.Count != 0)
            {
                lista.Add("otros");
            }

            lista.Add((p.TrabajoPuntual == true) ? "puntual" : "periodico");

        }
    }
}
