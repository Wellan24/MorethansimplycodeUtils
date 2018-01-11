using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaTomaMuestra
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("toma_muestra")]
    public class TomaMuestra : PersistenceData
    {
        [ColumnProperties("id_tomamuestra", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("fecha_tomamuestra")]
        public DateTime Fecha { get; set; }

        [ColumnProperties("lugar_tomamuestra")]
        public String Lugar { get; set; }

        [ColumnProperties("duracion_tomamuestra")]
        public String Duracion { get; set; }

        [ColumnProperties("tipo_muestra")]
        public String Tipo { get; set; }

        [ColumnProperties("volumen_tomamuestra")]
        public float Volumen { get; set; }

        [ColumnProperties("udsvolumen_tomamuestra")]
        public String UdsVolumen { get; set; }

        [ColumnProperties("numeroalicuotas_tomamuestra")]
        public int NumeroAlicuotas { get; set; }

        [ColumnProperties("idtecnico_tomamuestra")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idmuestra_tomamuestra")]
        public int IdMuestra { get; set; }
}
}
