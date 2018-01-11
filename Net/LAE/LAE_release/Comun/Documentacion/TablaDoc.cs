using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Documentacion
{
    public class TablaDoc
    {
        public String Titulo { get; set; }
        public Table Tabla { get; set; }
        public String Observaciones { get; set; }
    }
}
