using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaDiametroPelet
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.diametros_pelet")]
    public class DiametroPelet : PersistenceData, IModelo
    {
        [ColumnProperties("id_diametrospelet", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("medida_diametrospelet")]
        public double? Medida { get; set; }

        [ColumnProperties("idudsmedida_diametrospelet")]
        public int IdUdsMedida { get; set; }

        [ColumnProperties("idclase_diametrospelet")]
        public int IdClase { get; set; }

        public int Numero { get; set; }
        public String DiamNumero
        {
            get { return "D" + Numero; }
        }
    }
}