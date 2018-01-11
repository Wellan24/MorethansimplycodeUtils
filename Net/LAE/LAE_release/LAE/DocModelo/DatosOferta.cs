using Cartif.Collections;
using LAE.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAE.Comun.Modelo;

namespace LAE.DocModelo
{
    class DatosOferta
    {
        public static void AddData(Oferta o, RevisionOferta r, CartifDictionary<string, string> lista, Trabajo t = null)
        {
            lista.Add("añooferta", o.AnnoOferta.Year.ToString());
            lista.Add("codoferta", o.Codigo + "/" + r.NumCodigo);
            lista.Add("fecharevision", (r.FechaEmision == null) ? "" : (r.FechaEmision ?? DateTime.Now).ToString("dd/MM/yyyy"));
            lista.Add("fecharevision2", (r.FechaEmision == null) ? "" : (r.FechaEmision ?? DateTime.Now).ToString("dd 'de' MMMM 'de' yyyy"));
            lista.Add("importeoferta", r.Importe.ToString());
            lista.Add("numrevision", r.Num.ToString());
            lista.Add("condicionesrevision", r.Observaciones);
            lista.Add("importerevision", PersistenceManager.SelectByProperty<PuntocontrolRevision>("IdRevision", r.Id).Sum(pc => pc.Importe).ToString());
            lista.Add("plazooferta", r.PlazoRealizacion);
            if (t != null)
            {
                lista.Add("observacionestrabajo", t.Observaciones);
                lista.Add("codsolicitud", o.Codigo + "/" + r.NumCodigo + "-SE-" + t.NumCodigo);
                lista.Add("fechafirmalae", t.FechaFirmaLae.ToString("dd/MM/yyyy"));
                lista.Add("fechafirmacliente", t.FechaFirmaCliente.ToString("dd/MM/yyyy"));
            }
        }
    }
}
