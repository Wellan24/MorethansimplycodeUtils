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

namespace LAE.Comun.Documentacion
{
    public class Escritor
    {
        public static readonly String DocumentoOferta = "word/O-LAE-.docx";
        public static readonly String DocumentoSolicitudEnsayo = "word/SE-LAE-.docx";

        public Escritor(String rutaOriginal, Documento documento)
        {
            System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog() { Filter = "Word|*.docx", FileName = documento.nombreDocumento };

            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveDialog.FileName != "")
            {

                String copia = saveDialog.FileName;
                try
                {
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
                            KeyValuePair<string, TablaDoc[]>[] tablas = documento.ObtenerTablas(bookmark.Name);
                            if (tablas != null)
                            {
                                foreach (KeyValuePair<string, TablaDoc[]> datos in tablas)
                                {
                                    parent.InsertBeforeSelf(EscribirSaltoLinea());
                                    parent.InsertBeforeSelf(EscribirParagrafo(datos.Key, RunPropertiesEnum.Encabezados));

                                    foreach (TablaDoc tabla in datos.Value)
                                    {
                                        if (tabla.Titulo != null)
                                        {
                                            parent.InsertBeforeSelf(EscribirSaltoLinea());
                                            parent.InsertBeforeSelf(EscribirParagrafo(tabla.Titulo, RunPropertiesEnum.Encabezados));


                                        }
                                        parent.InsertBeforeSelf(EscribirSaltoLinea());
                                        parent.InsertBeforeSelf(tabla.Tabla);

                                        if (tabla.Observaciones != null)
                                        {
                                            parent.InsertBeforeSelf(EscribirSaltoLinea());
                                            parent.InsertBeforeSelf(EscribirParagrafo(tabla.Observaciones, RunPropertiesEnum.TextoNormal));
                                        }
                                    }
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

                    /* filas en tablas */
                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
                    {
                        foreach (BookmarkStart bookmark in wordDoc.MainDocumentPart.Document.Body.Descendants<BookmarkStart>())
                        {
                            if (bookmark != null)
                            {
                                TableRow tableRowClone = bookmark.Parent.Parent.Parent as TableRow; /* paragraph-> tablecell-> tablerow*/

                                List<TableRow> tablas = documento.ObtenerFilas(bookmark.Name, tableRowClone);
                                if (tablas.Count > 0)
                                {
                                    tablas.ForEach(t => tableRowClone.InsertBeforeSelf(t));
                                    tableRowClone.Remove();
                                }
                            }

                        }
                    }
                    MessageBox.Show("Fichero creado");
                }
                catch (Exception)
                {
                    MessageBox.Show("El fichero que intenta sobrescribir está abierto. Ciérrelo si desea sobrescribirlo");
                }
            }
        }

        public static Paragraph EscribirParagrafo(string text, RunProperties properties = null)
        {
            Paragraph p = new Paragraph();
            Run run = p.AppendChild(new Run());
            if (properties != null)
                run.AppendChild(properties);


            if (!text.Contains("\r\n"))
                run.AppendChild(new Text(text));
            else
            {
                string[] lineas = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string linea in lineas)
                {
                    run.AppendChild(new Text(linea));
                    run.AppendChild(new Break());
                }
            }
            return p;
        }

        public static Paragraph EscribirSaltoLinea()
        {
            Paragraph p = new Paragraph();
            Run run = p.AppendChild(new Run());
            run.AppendChild(RunPropertiesEnum.SaltoLinea);
            return p;
        }
        
    }
}
