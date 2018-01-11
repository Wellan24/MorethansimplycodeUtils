using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class ILineasParametros
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public String Metodo { get; set; }
        public int IdParametro { get; set; }
        public int IdRelacion { get; set; }
    }
}
