using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Word_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Hashtable datos = new Hashtable
            //{
            //    "N"="N"
            //};

            //InitializeComponent();
            //Leer("word/O-LAE.docx");
            //ReemplazarTextoTodo("word/prueba6.docx");
            //ReemplazarTextoByTabla2("word/prueba5.docx");
            //ReemplazarTextoByTabla("word/prueba4.docx");
            //ReemplazarTexto2("word/prueba2.docx");
            //Leer("word/prueba0.docx");
            //CrearDoc("word/prueba1.docx");
            //ReemplazarTexto("word/prueba2.docx");
            CrearTabla("word/prueba3.docx");
        }

        private void CrearTabla(string docName)
        {
            // Use the file name and path passed in as an argument 
            // to open an existing Word 2007 document.
            string copia = "word/prueba3_copia.docx";
            File.Copy(docName, copia, true);

            using (WordprocessingDocument doc = WordprocessingDocument.Open(copia, true))
            {
                // Create an empty table.
                Table table = new Table();

                // Create a TableProperties object and specify its border information.
                // Create a TableProperties object and specify its border information.
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder()
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Single),
                            Size = 12
                        },
                        new BottomBorder()
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Single),
                            Size = 12
                        },
                        new LeftBorder()
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Single),
                            Size = 12
                        },
                        new RightBorder()
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Single),
                            Size = 12
                        },
                        new InsideHorizontalBorder()
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Single),
                            Size = 12
                        },
                        new InsideVerticalBorder()
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Single),
                            Size = 12
                        }
                    )
                );

                // Append the TableProperties object to the empty table.
                table.AppendChild<TableProperties>(tblProp);

                // Create a row.
                TableRow tr = new TableRow();

                // Create a cell.
                TableCell tc1 = new TableCell();

                // Specify the width property of the table cell.
                tc1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "11000" }));

                // Specify the table cell content.
                tc1.Append(new ParagraphProperties(new Justification { Val = JustificationValues.Center }), new Paragraph(new Run(new RunProperties(new Bold()), new Text("some center"))));

                // Append the table cell to the table row.
                tr.Append(tc1);

                // Create a second table cell by copying the OuterXml value of the first table cell.
                TableCell tc2 = new TableCell();

                // Specify the width property of the table cell.
                tc2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "500" }));

                // Specify the table cell content.
                tc2.Append(new Paragraph(new Run(new Text("some text 1"))));

                // Append the table cell to the table row.
                tr.Append(tc2);

                // Append the table row to the table.
                table.Append(tr);


                // Create a row.
                tr = new TableRow();

                // Create a cell.
                tc1 = new TableCell();

                // Specify the width property of the table cell.
                //tc1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "1000" }));

                // Specify the table cell content.
                tc1.Append(new Paragraph(new Run(new Text("some text 2"))));

                // Append the table cell to the table row.
                tr.Append(tc1);

                // Create a second table cell by copying the OuterXml value of the first table cell.
                tc2 = new TableCell();

                // Specify the width property of the table cell.
                //tc2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "500" }));

                // Specify the table cell content.
                tc2.Append(new Paragraph(new Run(new Text("some text 3"))));

                // Append the table cell to the table row.
                tr.Append(tc2);


                // Append the table row to the table.
                table.Append(tr);

                /* MERGE */
                tr = new TableRow();
                tc1 = new TableCell();

                
                //HorizontalMerge ver = new HorizontalMerge() { Val = MergedCellValues.Continue };
                //tc1.Append(new TableCellProperties(ver));

                tc1.Append(new TableCellProperties(new GridSpan() { Val=2}));
                tc1.Append(new Paragraph(new Run(new Text("total2"))));

                tr.Append(tc1);

                table.Append(tr);

                /* FIN MERGE*/


                // Append the table to the document.
                doc.MainDocumentPart.Document.Body.Append(table);
            }

        }

        private void ReemplazarTextoTodo(string docName)
        {
            string copia = "word/prueba6_1.docx";
            File.Copy(docName, copia, true);

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
                    string s = item.Groups[1].ToString();
                    string t = obtener(s);
                    docText = docText.Replace(item.Value, t);
                }

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
                //var a = reg.Matches(docText)[0].Groups[1];
                //var b = reg.Matches(docText)[0].Value;
                { }
            }


        }

        private string obtener(string s)
        {
            if (s.Equals("texto1"))
                return "prueba1";
            else if (s.Equals("texto2"))
                return "prueba2";
            return "error";
        }

        private void ReemplazarTextoByTabla2(string docName)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docName, true))
            {

                var mainPart = wordDoc.MainDocumentPart;
                var res = from bm in mainPart.Document.Body.Descendants<BookmarkStart>()
                          where bm.Name == "texto1"
                          select bm;
                var bookmark = res.SingleOrDefault();
                if (bookmark != null)
                {
                    var parent = bookmark.Parent;

                    parent.InsertAfterSelf(CrearTabla2());
                }
                //wordDoc.Close();
            }
        }

        private void ReemplazarTextoByTabla(string docName)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docName, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("#texto1#");
                docText = regexText.Replace(docText, CrearTabla2().ToString());

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        private void Leer(string docName)
        {
            try
            {
                using (WordprocessingDocument myDocument = WordprocessingDocument.Open(docName, true))
                {
                    Body body = myDocument.MainDocumentPart.Document.Body;
                    string content = body.InnerText;
                    string content2 = body.InnerXml;
                    { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private Table CrearTabla2()
        {
            // Use the file name and path passed in as an argument 
            // to open an existing Word 2007 document.


            // Create an empty table.
            Table table = new Table();

            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder()
                    {
                        Val =
                            new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 24
                    },
                    new BottomBorder()
                    {
                        Val =
                            new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 24
                    },
                    new LeftBorder()
                    {
                        Val =
                            new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 24
                    },
                    new RightBorder()
                    {
                        Val =
                            new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 24
                    },
                    new InsideHorizontalBorder()
                    {
                        Val =
                            new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 24
                    },
                    new InsideVerticalBorder()
                    {
                        Val =
                            new EnumValue<BorderValues>(BorderValues.Single),
                        Size = 24
                    }
                )
            );

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(tblProp);

            // Create a row.
            TableRow tr = new TableRow();

            // Create a cell.
            TableCell tc1 = new TableCell();

            // Specify the width property of the table cell.
            tc1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Auto, Width = "1200" }));

            // Specify the table cell content.
            tc1.Append(new Paragraph(new Run(new Text("some text"))));

            // Append the table cell to the table row.
            tr.Append(tc1);

            // Create a second table cell by copying the OuterXml value of the first table cell.
            TableCell tc2 = new TableCell(tc1.OuterXml);

            // Append the table cell to the table row.
            tr.Append(tc2);

            // Append the table row to the table.
            table.Append(tr);

            // Append the table to the document.
            return table;


        }

        private void ReemplazarTexto(string docName)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(docName, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("#texto1#");
                docText = regexText.Replace(docText, "Texto substituido");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        private void ReemplazarTexto2(string docName)
        {
            string copia = "word/prueba2_1.docx";
            System.IO.File.Copy(docName, copia);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(copia, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("#texto1#");
                docText = regexText.Replace(docText, "Texto substituido");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        private void CrearDoc(string docName)
        {
            // Create a document. 
            // Create a Wordprocessing document. 
            using (WordprocessingDocument package = WordprocessingDocument.Create(docName, WordprocessingDocumentType.Document))
            {
                // Add a new main document part. 
                package.AddMainDocumentPart();

                // Create the Document DOM. 
                package.MainDocumentPart.Document =
                  new Document(
                    new Body(
                      new Paragraph(
                        new Run(
                          new Text("Prueba1")))));

                // Save changes to the main document part. 
                package.MainDocumentPart.Document.Save();
            }
        }
    }
}
