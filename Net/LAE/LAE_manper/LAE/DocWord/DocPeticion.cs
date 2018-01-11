using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Collections;
using LAE.Modelo;
using LAE.Comun.Persistence;
using LAE.DocModelo;
using DocumentFormat.OpenXml.Wordprocessing;
using LAE.Comun.Documentacion;
using LAE.Comun.Modelo;

namespace LAE.DocWord
{
    class DocPeticion : Documento
    {
        public Peticion peticion { get; set; }

        public DocPeticion(Peticion p)
        {
            peticion = p;
            listaTextoReemplazar = new CartifDictionary<string, string>();
            listaChecks = new List<string>();
            listaFilasAdd = new CartifDictionary<string, string[,]>();

            GenerarTextoReemplazar();
            GenerarListaChecks();
            GenerarListaFilas();
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

        private void GenerarListaFilas()
        {
            listaFilasAdd.Add("filamuestras", GenerarListaParametros());
        }

        private string[,] GenerarListaParametros()
        {
            KeyValuePair<Parametro, int>[] parametros = FactoriaParametros.GetParametrosPeticionPorMuestra(peticion).ToArray();

            String[,] lista = new String[parametros.Count(), 3];
            for (int i = 0; i < parametros.Count(); i++)
            {
                lista[i, 0] = parametros[i].Value.ToString();
                lista[i, 1] = parametros[i].Key.NombreParametro;
                lista[i, 2] = parametros[i].Key.MetodoParametro;
            }
            return lista;
        }

    }
}
