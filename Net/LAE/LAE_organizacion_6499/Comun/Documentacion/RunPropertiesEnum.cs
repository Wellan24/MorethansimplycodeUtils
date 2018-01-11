using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Documentacion
{
    public class RunPropertiesEnum
    {
        public static RunProperties Encabezados
        {
            get
            {
                return new RunProperties(
                    new Bold(), 
                    new Color() { Val = "739C8D" }, 
                    new RunFonts() { Ascii = "Frutiger LT Std 45 Light", HighAnsi = "Frutiger LT Std 45 Light" }, 
                    new FontSize() { Val="28"},
                    new SmallCaps());
            }
        }

        public static RunProperties SaltoLinea
        {
            get
            {
                return new RunProperties(
                    new RunFonts() { Ascii = "Frutiger LT Std 45 Light", HighAnsi = "Frutiger LT Std 45 Light" },
                    new FontSize() { Val = "22" });
            }
        }

        public static RunProperties TextoNormal
        {
            get
            {
                return new RunProperties(
                    new RunFonts() { Ascii = "Frutiger LT Std 45 Light", HighAnsi = "Frutiger LT Std 45 Light" },
                    new FontSize() { Val = "22" });
            }
        }


    }
}
