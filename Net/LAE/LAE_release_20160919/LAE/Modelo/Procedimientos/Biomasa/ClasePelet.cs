using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaClasePelet
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.clase_pelet")]
    public class ClasePelet : PersistenceData, IModelo
    {
        [ColumnProperties("id_clasepelet", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("porcentaje_clasepelet")]
        public int Porcentaje { get; set; }

        [ColumnProperties("iddiametro_clasepelet")]
        public int IdDiametro { get; set; }

        [ColumnProperties("iddimension_clasepelet")]
        public int IdDimension { get; set; }

        public double? MediaLongitud { get; set; }
        public double? DesviacionLongitud { get; set; }
        public double? MediaDiametro { get; set; }
        public double? DesviacionDiametro { get; set; }

        public List<LongitudPelet> Longitudes;
        public List<DiametroPelet> Diametros;
    }
}