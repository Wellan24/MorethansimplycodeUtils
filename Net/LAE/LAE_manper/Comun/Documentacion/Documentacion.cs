using Cartif.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Documentacion
{
    public class Documento
    {
        public CartifDictionary<string, string> listaTextoReemplazar { get; set; }
        public CartifDictionary<string, KeyValuePair<String, TablaDoc[]>[]> listaTablaDocReemplazar { get; set; }
        public List<String> listaChecks { get; set; }
        public CartifDictionary<string, string[,]> listaFilasAdd { get; set; }
        public String nombreDocumento { get; set; }

        public Documento()
        {
            listaTextoReemplazar = new CartifDictionary<string, string>();
            listaTablaDocReemplazar = new CartifDictionary<string, KeyValuePair<String, TablaDoc[]>[]>();
            listaChecks = new List<string>();
            listaFilasAdd = new CartifDictionary<string, string[,]>();
        }

        public string ObtenerTexto(string marcador)
        {
            return listaTextoReemplazar[marcador]?.Replace("\r\n","<w:br/>");
        }

        public KeyValuePair<String, TablaDoc[]>[] ObtenerTablas(string marcador)
        {
            return listaTablaDocReemplazar[marcador];
        }

        public bool IsChecked(String marcador)
        {
            if (listaChecks.Contains(marcador))
                return true;
            return false;
        }

        public List<TableRow> ObtenerFilas(String marcador, TableRow clone)
        {
            List<TableRow> filas = new List<TableRow>();
            TableRow tr;
            TableCell tc;
            String[,] lista = listaFilasAdd[marcador];
            if (lista != null)
            {
                for (int i = 0; i < lista.GetLength(0); i++)
                {
                    tr = new TableRow(clone.OuterXml);
                    for (int j = 0; j < lista.GetLength(1); j++)
                    {
                        tc = tr.Elements<TableCell>().ElementAt(j);
                        tc.Append(new Paragraph(new Run(new Text(lista[i, j]))));
                    }
                    filas.Add(tr);
                }
                return filas;
            }
            return new List<TableRow>();
        }
    }
}
