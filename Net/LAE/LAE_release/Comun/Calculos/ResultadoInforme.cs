using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Calculos
{
    public class ResultadoInforme
    {
        public String Valor { get; set; }

        private String incertidumbre;
        public String Incertidumbre
        {
            get
            {
                if (incertidumbre == null || incertidumbre.Equals(String.Empty))
                    return "--";
                return "\u00B1" + incertidumbre;
            }
            set { incertidumbre = value?.Replace("-", "")?.Replace("\u00B1", ""); }
        }

        public int IdVProcedimiento { get; set; }

        public int IdParametro { get; set; }

        /*Para el Tooltip del Control*/
        public String RangoIncertidumbre { get; set; }

        public String Alcance { get; set; }
    }
}
