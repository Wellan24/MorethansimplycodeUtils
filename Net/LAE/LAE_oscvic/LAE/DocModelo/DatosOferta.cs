using Cartif.Collections;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocModelo
{
    class DatosOferta
    {
        public static void AddData(Oferta o, RevisionOferta r, CartifDictionary<string, string> lista)
        {
            lista.Add("añooferta", o.AnnoOferta.ToString());
            lista.Add("codoferta", o.Codigo+"/"+r.Num);
            lista.Add("fechaoferta", (r.FechaEmision ?? DateTime.Now).ToString("dd/MM/yyyy"));
            lista.Add("fechaoferta2", (r.FechaEmision ?? DateTime.Now).ToString("dd 'de' MMMM 'de' yyyy"));
            lista.Add("importeoferta", r.Importe.ToString());
            lista.Add("numrevision", r.Num.ToString());
            lista.Add("codsolicitud", String.Format("SE-LAE-{0}/{1:00#}", (o.AnnoOferta - (o.AnnoOferta / 100) * 100), o.NumCodigoOferta));
        }
    }
}
