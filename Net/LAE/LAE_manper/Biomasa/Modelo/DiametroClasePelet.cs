using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaDiametroClasePelet
    {
        private static DiametroClasePelet[] diametrosClase = null;
        public static DiametroClasePelet[] GetDiametrosClase()
        {
            if (diametrosClase == null)
                diametrosClase = PersistenceManager.SelectAll<DiametroClasePelet>().ToArray();
            return diametrosClase;
        }
    }

    [TableProperties("biomasa.diametrosclase_pelet")]
    public class DiametroClasePelet : PersistenceData, IModelo
    {
        [ColumnProperties("id_diametrosclasepelet", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_diametrosclasepelet")]
        public String Nombre { get; set; }

        [ColumnProperties("medida_diametrosclasepelet")]
        public double? Medida { get; set; }

        [ColumnProperties("idudsmedida_diametrosclasepelet")]
        public int? IdUdsMedida { get; set; }
    }
}