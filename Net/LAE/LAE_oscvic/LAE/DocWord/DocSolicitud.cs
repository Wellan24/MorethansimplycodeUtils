using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Collections;
using LAE.Modelo;
using Persistence;
using LAE.DocModelo;

namespace LAE.DocWord
{
    class DocSolicitud : IDocumentacion
    {
        public RevisionOferta revision { get; set; }
        public CartifDictionary<string, string> listaTextoReemplazar { get; set; }
        public CartifDictionary<string, TablaDoc[]> listaTablaDocReemplazar { get; set; }
        public String nombreDocumento { get; set; }

        public DocSolicitud(RevisionOferta r)
        {
            revision = r;
            GenerarTextoReemplazar();
            GenerarTablaDoc();
        }

        private void GenerarTextoReemplazar()
        {
            listaTextoReemplazar = new CartifDictionary<string, string>();

            Oferta o = PersistenceManager.SelectByProperty<Oferta>("Id", revision.IdOferta).FirstOrDefault();
            DatosOferta.AddData(o, revision, listaTextoReemplazar);

            Contacto c = PersistenceManager.SelectByProperty<Contacto>("Id", o.IdContacto).FirstOrDefault();
            DatosContacto.AddData(c, listaTextoReemplazar);

            Cliente cl = PersistenceManager.SelectByProperty<Cliente>("Id", o.IdCliente).FirstOrDefault();
            DatosCliente.AddData(cl, listaTextoReemplazar);

            nombreDocumento = String.Format("SE-LAE-{0}-{1:00#}", (o.AnnoOferta - (o.AnnoOferta / 100) * 100), o.NumCodigoOferta);
        }

        private void GenerarTablaDoc()
        {
            listaTablaDocReemplazar = new CartifDictionary<string, TablaDoc[]>();
        }

        public string ObtenerTexto(string marcador)
        {
            return listaTextoReemplazar[marcador];
        }

        public TablaDoc[] ObtenerTablas(string marcador)
        {
            return listaTablaDocReemplazar[marcador];
        }

        public bool IsChecked(string marcador)
        {
            throw new NotImplementedException();
        }
    }
}
