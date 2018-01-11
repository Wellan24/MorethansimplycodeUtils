using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Collections;
using LAE.Modelo;
using Persistence;
using LAE.DocModelo;
using DocumentFormat.OpenXml.Wordprocessing;

namespace LAE.DocWord
{
    class DocSolicitud : Documentacion
    {
        //public RevisionOferta revision { get; set; }
        public Trabajo trabajo { get; set; }

        public DocSolicitud(Trabajo t)
        {
            trabajo = t;
            GenerarTextoReemplazar();
            GenerarTablaDoc();
        }

        private void GenerarTextoReemplazar()
        {
            Oferta o = PersistenceManager.SelectByProperty<Oferta>("Id", trabajo.IdOferta).FirstOrDefault();
            RevisionOferta r = PersistenceManager.SelectByProperty<RevisionOferta>("IdOferta", o.Id).Where(re => re.Aceptada == true).FirstOrDefault();
            DatosOferta.AddData(o, r, listaTextoReemplazar, trabajo);

            Contacto c = PersistenceManager.SelectByProperty<Contacto>("Id", o.IdContacto).FirstOrDefault();
            DatosContacto.AddData(c, listaTextoReemplazar);

            Cliente cl = PersistenceManager.SelectByProperty<Cliente>("Id", o.IdCliente).FirstOrDefault();
            DatosCliente.AddData(cl, listaTextoReemplazar);

            //nombreDocumento = o.Codigo+"-"+r.NumCodigo+"-SE-"+trabajo.NumCodigo;
            nombreDocumento = o.Codigo + "-SE-" + trabajo.NumCodigo;
        }

        private void GenerarTablaDoc()
        {
            
        }
        
    }
}
