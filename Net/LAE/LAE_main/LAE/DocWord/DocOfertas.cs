using Cartif.Collections;
using LAE.DocModelo;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace LAE.DocWord
{
    class DocOfertas : IDocumentacion
    {
        public RevisionOferta revision { get; set; }
        public CartifDictionary<string, string> listaTextoReemplazar { get; set; }
        public CartifDictionary<string, TablaDoc[]> listaTablaDocReemplazar { get; set; }
        public String nombreDocumento { get; set; }

        public DocOfertas(RevisionOferta r)
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

            Tecnico t = PersistenceManager.SelectByProperty<Tecnico>("Id", revision.IdTecnico).FirstOrDefault();
            DatosTecnico.AddData(t, listaTextoReemplazar);

            nombreDocumento = o.Codigo + "-" + revision.Num;
        }

        private void GenerarTablaDoc()
        {
            listaTablaDocReemplazar = new CartifDictionary<string, TablaDoc[]>();
            listaTablaDocReemplazar.Add("listaensayos", GenerarListaEnsayos());
        }

        private TablaDoc[] GenerarListaEnsayos()
        {
            TipoMuestra[] tipos = FactoriaTipoMuestra.GetMuestrasRevision(revision).ToArray();
            TablaDoc[] tabDoc = new TablaDoc[tipos.Count()];

            for (int i = 0; i < tipos.Count(); i++)
            {
                KeyValuePair<Parametro, int>[] parametros = FactoriaParametros.GetParametrosRevisionPorMuestra(revision, tipos[i]).ToArray();
                tabDoc[i] = new TablaDoc
                {
                    Titulo = String.Format("4.{0} ANÁLISIS {1}", (i + 1), tipos[i].Nombre.ToUpper()),
                    Tabla = GenerarTablaEnsayos(parametros, revision)
                };
            }
            return tabDoc;
        }

        private Table GenerarTablaEnsayos(KeyValuePair<Parametro, int>[] parametros, RevisionOferta rev)
        {
            // Create an empty table.
            Table table = new Table();

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(TablePropertiesEnum.tablaGenerica(12));

            TableRow tr;
            Parametro p;

            /* Cabecera */
            tr = new TableRow();
            tr.Append(CeldaTabla.CeldaFormat("Cantidades").Color("739C8D").Size(1000).Bold().Build());
            tr.Append(CeldaTabla.CeldaFormat("Descripción (Parámetro, actividad, método)").Color("739C8D").Size(8000).Bold().Build());
            table.Append(tr);

            /* Cuerpo */
            String txt;
            foreach (KeyValuePair<Parametro, int> param in parametros)
            {
                tr = new TableRow();
                txt = param.Value.ToString();
                tr.Append(CeldaTabla.CeldaFormat(txt).Justification(JustificationValues.Center).Build());

                p = param.Key;
                txt = p.NombreParametro + ((p.MetodoParametro != null) ? " (" + p.MetodoParametro + ")" : "") + ((p.Norma != null) ? " (" + p.Norma + ")" : "");
                tr.Append(CeldaTabla.CeldaFormat(txt).Build());

                table.Append(tr);
            }

            /* Pie */
            tr = new TableRow();
            txt = String.Format("Total ... {0} euros", rev.Importe);
            tr.Append(CeldaTabla.CeldaFormat(txt).GridSpan(2).Justification(JustificationValues.Center).Bold().Build());
            table.Append(tr);

            return table;
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
