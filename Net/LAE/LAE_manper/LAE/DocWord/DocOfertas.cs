using Cartif.Collections;
using LAE.DocModelo;
using LAE.Modelo;
using LAE.Comun.Persistence;
using LAE.Comun.Documentacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Cartif.Extensions;
using LAE.Comun.Modelo;

namespace LAE.DocWord
{
    class DocOfertas : Documento
    {
        public RevisionOferta revision { get; set; }

        public DocOfertas(RevisionOferta r)
        {
            revision = r;
            GenerarTextoReemplazar();
            GenerarTablaDoc();
        }

        private void GenerarTextoReemplazar()
        {

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
            listaTablaDocReemplazar.Add("listaensayos", GenerarListaParametros());
        }

        private KeyValuePair<String, TablaDoc[]>[] GenerarListaParametros()
        {
            TipoMuestra[] tipos = FactoriaTipoMuestra.GetMuestrasRevision(revision).ToArray();

            KeyValuePair<string, TablaDoc[]>[] tablasPorTipos = new KeyValuePair<string, TablaDoc[]>[tipos.Count()];

            TablaDoc[] tabla;
            for (int i = 0; i < tipos.Count(); i++)
            {
                Tuple<Parametro, int, PuntocontrolRevision>[] parametros = FactoriaParametros.GetParametrosRevisionPorMuestra2(revision, tipos[i]).ToArray();
                int[] idsPuntosControl = parametros.GroupBy(p => p.Item3.Id).Select(p => p.Key).ToArray();

                tabla = new TablaDoc[idsPuntosControl.Count()];
                for (int j = 0; j < idsPuntosControl.Count(); j++)
                {
                    var param=parametros.Where(p => p.Item3.Id == idsPuntosControl[j]);
                    tabla[j] = new TablaDoc
                    {
                        Titulo = param.Select(p => p.Item3.Nombre).FirstOrDefault(),
                        Tabla = GenerarTablaEnsayos(param.Select(p => new KeyValuePair<Parametro, int>(p.Item1, p.Item2)).ToArray(), param.Select(p => p.Item3).FirstOrDefault()),
                        Observaciones = param.Select(p => p.Item3.Observaciones).FirstOrDefault()
                    };

                }

                tablasPorTipos[i] = new KeyValuePair<string, TablaDoc[]>(String.Format("4.{0} Análisis {1}", (i + 1), tipos[i].Nombre), tabla);

            }
            return tablasPorTipos;
        }

        private Table GenerarTablaEnsayos(KeyValuePair<Parametro, int>[] parametros, PuntocontrolRevision pc)
        {
            // Create an empty table.
            Table table = new Table();

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(TablePropertiesEnum.TablaGenerica(12));

            TableRow tr;
            //TableRowProperties trProperties;
            Parametro p;

            /* Cabecera */
            tr = FilaTabla.Fila().Height(750).Build();
            tr.Append(CeldaTabla.CeldaFormat("Cantidad").Color("739C8D").WidthCell(1500).Bold().Build());
            tr.Append(CeldaTabla.CeldaFormat("Descripción (Parámetro, actividad, método)").Color("739C8D").WidthCell(8000).Bold().Build());
            table.Append(tr);

            /* Cuerpo */
            String txt;
            foreach (KeyValuePair<Parametro, int> param in parametros)
            {
                tr = tr = FilaTabla.Fila().Height(450).Build();
                txt = param.Value.ToString();
                tr.Append(CeldaTabla.CeldaFormat(txt).Justification(JustificationValues.Center).Build());

                p = param.Key;
                txt = p.NombreParametro + ((p.MetodoParametro != null) ? " (" + p.MetodoParametro + ")" : "") + ((p.Norma != null) ? " (" + p.Norma + ")" : "");
                tr.Append(CeldaTabla.CeldaFormat(txt).Font("Frutiger LT Std 45 Light").Build());

                table.Append(tr);
            }

            /* Pie */
            tr = tr = FilaTabla.Fila().Height(500).Build();
            txt = String.Format("Total (sin I.V.A) ... {0} euros", pc.Importe);
            tr.Append(CeldaTabla.CeldaFormat(txt).GridSpan(2).Justification(JustificationValues.Center).Bold().Build());
            table.Append(tr);

            return table;
        }

    }
}
