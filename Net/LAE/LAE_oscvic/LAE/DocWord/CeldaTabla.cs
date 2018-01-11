using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.DocWord
{
    public class CeldaTabla
    {
        private TableCell cell;
        private TableCellProperties properties;
        private RunProperties runProperties;
        private ParagraphProperties paragraphProperties;

        public CeldaTabla()
        {
            cell = new TableCell();
            properties = new TableCellProperties();
            runProperties = new RunProperties();
            paragraphProperties = new ParagraphProperties();
        }

        public CeldaTabla Texto(String text)
        {
            /* importante el orden, 1º paragraphproperties, después escribir paragrafo */
            cell.Append(paragraphProperties);
            cell.Append(new Paragraph(new Run(runProperties, new Text(text))));
            cell.Append(properties);

            return this;
        }

        /* tablecellProperties */
        public CeldaTabla Color(String color)
        {
            properties.Append(new Shading() { Color = "auto", Fill = color });
            return this;
        }

        public CeldaTabla Size(int size)
        {
            properties.Append(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = size.ToString() });
            return this;
        }

        public CeldaTabla VerticalAlignment(TableVerticalAlignmentValues value)
        {
            properties.Append(new TableCellVerticalAlignment() { Val = value });
            return this;
        }

        public CeldaTabla Margin(int top, int bottom)
        {
            properties.Append(new TableCellMargin(new TopMargin() { Width = top.ToString() }, new BottomMargin() { Width = bottom.ToString() }));
            return this;
        }

        public CeldaTabla GridSpan(int value)
        {
            properties.Append(new GridSpan() { Val = value });
            return this;
        }

        public CeldaTabla VerticalMerge(MergedCellValues value)
        {
            properties.Append(new VerticalMerge() { Val = value });
            return this;
        }

        public CeldaTabla Border()
        {
            properties.Append(new TableCellBorders(
                new LeftBorder()
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Nil)
                }
            ));
            return this;
        }

        public CeldaTabla BottomBorder(int value)
        {
            BottomBorder bottomBorder = new BottomBorder();
            return GenerarBorde(bottomBorder, value);

        }

        public CeldaTabla TopBorder(int value)
        {
            TopBorder topBorder = new TopBorder();
            return GenerarBorde(topBorder, value);

        }

        public CeldaTabla LeftBorder(int value)
        {
            LeftBorder leftBorder = new LeftBorder();
            return GenerarBorde(leftBorder, value);

        }

        public CeldaTabla RightBorder(int value)
        {
            RightBorder rightBorder = new RightBorder();
            return GenerarBorde(rightBorder, value);

        }

        public CeldaTabla GenerarBorde(BorderType border, int value)
        {
            if (value == 0)
                border.Val = new EnumValue<BorderValues>(BorderValues.Nil);
            else
            {
                border.Val = new EnumValue<BorderValues>(BorderValues.Single);
                border.Size = Convert.ToUInt32(value);
            }

            properties.Append(border);
            return this;
        }

        /* runProperties */
        public CeldaTabla Bold()
        {
            runProperties.Append(new Bold());
            return this;
        }

        /* paragraphProperties */
        public CeldaTabla Justification(JustificationValues value)
        {
            paragraphProperties.Append(new Justification() { Val = value });
            return this;
        }

        public TableCell Build()
        {
            return cell;
        }

        public static CeldaTabla Celda(String texto)
        {
            return new CeldaTabla().Texto(texto);
        }

        public static CeldaTabla CeldaFormat(String texto)
        {
            return Celda(texto).VerticalAlignment(TableVerticalAlignmentValues.Center);
        }
    }
}
