using Cartif.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocWord
{
    interface IDocumentacion
    {
        CartifDictionary<string, string> listaTextoReemplazar { get; set; }
        CartifDictionary<string, TablaDoc[]> listaTablaDocReemplazar { get; set; }
        String nombreDocumento { get; set; }
        
        String ObtenerTexto(String marcador);
        TablaDoc[] ObtenerTablas(String marcador);
        bool IsChecked(String marcador);
    }
}
