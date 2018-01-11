using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaLineas
    {

    }

    public class Linea
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int IdParametro { get; set; }
        public int IdPControlPeticion { get; set; }
        public int Tipo { get; set; }
    }
}
