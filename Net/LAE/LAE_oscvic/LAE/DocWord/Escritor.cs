using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using System.Windows;

namespace LAE.DocWord
{
    class Escritor
    {
        public static readonly String DocumentoOferta = "word/O-LAE.docx"; /* viejo */
        public static readonly String DocumentoSolicitudEnsayo = "word/F-PGG-7.2.0-06_Rev7.docx";
        public static readonly String DocumentoPeticion = "word/F-PGG-7.2.0-07_Rev3-peticion.docx";

        public Escritor(String rutaOriginal, String rutaDestino, IDocumentacion documento)
        {
            String copia = rutaDestino + "/" + documento.nombreDocumento + ".docx";
            File.Copy(rutaOriginal, copia, true);

            /* Reemplazar texto */
            Regex reg = new Regex(@"##([A-Za-z0-9ñÑ]+)\$\$");
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }
                foreach (Match item in reg.Matches(docText))
                {
                    string bookmark = item.Groups[1].ToString();
                    string textToReplace = documento.ObtenerTexto(bookmark);
                    docText = docText.Replace(item.Value, textToReplace);
                }

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }

            /* header */
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
            {
                string docText = null;
                foreach (var header in wordDoc.MainDocumentPart.HeaderParts)
                {
                    using (StreamReader sr = new StreamReader(header.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                    foreach (Match item in reg.Matches(docText))
                    {
                        string bookmark = item.Groups[1].ToString();
                        string textToReplace = documento.ObtenerTexto(bookmark);
                        docText = docText.Replace(item.Value, textToReplace);
                    }

                    using (StreamWriter sw = new StreamWriter(header.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                }
            }

            /* TablaDoc */
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
            {
                foreach (var bookmark in wordDoc.MainDocumentPart.Document.Body.Descendants<BookmarkStart>())
                {
                    OpenXmlElement parent = bookmark.Parent;
                    TablaDoc[] tablas = documento.ObtenerTablas(bookmark.Name);
                    if (tablas != null)
                    {
                        foreach (TablaDoc datos in tablas)
                        {
                            if (datos.Titulo != null)
                            {
                                /* salto de linea */
                                Paragraph salto = new Paragraph();
                                parent.InsertBeforeSelf(salto);

                                /* Con formato */
                                Paragraph p = new Paragraph();
                                Run run = p.AppendChild(new Run());
                                RunProperties runProperties = run.AppendChild(new RunProperties(new Bold(), new Color() { Val = "739C8D" }));
                                run.AppendChild(new Text(datos.Titulo));
                                parent.InsertBeforeSelf(p);

                                /* Sin formato */
                                //Paragraph p = new Paragraph(new Run(new Text(datos.Titulo)));
                                //parent.InsertBeforeSelf(p);
                            }
                            parent.InsertBeforeSelf(datos.Tabla);
                        }
                    }
                }
            }

            /* Checkbox */
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
            {
                foreach (CheckBox cb in wordDoc.MainDocumentPart.Document.Body.Descendants<CheckBox>())
                {
                    FormFieldName cbName = cb.Parent.ChildElements.First<FormFieldName>();

                    bool marcar = documento.IsChecked(cbName.Val.Value);
                    DefaultCheckBoxFormFieldState dcb = cb.GetFirstChild<DefaultCheckBoxFormFieldState>();
                    dcb.Val = marcar;
                }
            }

            /* prueba tablas */
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
            {
                
            }

            MessageBox.Show("Fichero creado");

        }



    }
}
