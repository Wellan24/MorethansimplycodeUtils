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
    class DocPeticion : IDocumentacion
    {
        public Peticion peticion { get; set; }
        public CartifDictionary<string, string> listaTextoReemplazar { get; set; }
        public CartifDictionary<string, TablaDoc[]> listaTablaDocReemplazar { get; set; }
        public List<String> listaChecks { get; set; }
        public String nombreDocumento { get; set; }

        public DocPeticion(Peticion p)
        {
            peticion = p;
            listaTextoReemplazar = new CartifDictionary<string, string>();
            listaTablaDocReemplazar = new CartifDictionary<string, TablaDoc[]>();
            listaChecks = new List<string>();

            GenerarTextoReemplazar();
            //GenerarTablaDoc();
            GenerarListaChecks();
            nombreDocumento = "peticion";
        }

        private void GenerarTextoReemplazar()
        {
            DatosPeticion.AddData(peticion, listaTextoReemplazar);

            Contacto c = PersistenceManager.SelectByProperty<Contacto>("Id", peticion.IdContacto).FirstOrDefault();
            DatosContacto.AddData(c, listaTextoReemplazar);

            Cliente cl = PersistenceManager.SelectByProperty<Cliente>("Id", peticion.IdCliente).FirstOrDefault();
            DatosCliente.AddData(cl, listaTextoReemplazar);
        }

        private void GenerarListaChecks()
        {
            
            DatosPeticion.AddListaCheck(peticion, listaChecks);
        }

        public string ObtenerTexto(string marcador)
        {
            return listaTextoReemplazar[marcador];
        }

        public TablaDoc[] ObtenerTablas(string marcador)
        {
            return listaTablaDocReemplazar[marcador];
        }

        public bool IsChecked(String marcador)
        {
            if (listaChecks.Contains(marcador))
                return true;
            return false;
        }
    }
}
