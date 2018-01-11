using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Documentacion
{
    public class TablePropertiesEnum
    {
        public static TableProperties TablaGenerica(int borde)
        {
            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder()
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = Convert.ToUInt32(borde)
                    },
                    new BottomBorder()
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = Convert.ToUInt32(borde)
                    },
                    new LeftBorder()
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = Convert.ToUInt32(borde)
                    },
                    new RightBorder()
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = Convert.ToUInt32(borde)
                    },
                    new InsideHorizontalBorder()
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = Convert.ToUInt32(borde)
                    },
                    new InsideVerticalBorder()
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Single),
                        Size = Convert.ToUInt32(borde)
                    }
                )
            );
            return tblProp;
        }
    }
}
