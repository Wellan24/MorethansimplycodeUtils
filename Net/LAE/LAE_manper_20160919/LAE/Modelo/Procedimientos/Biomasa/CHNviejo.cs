using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaCHNviejo
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.CHN")]
    public class CHNviejo : PersistenceData
    {
        [ColumnProperties("id_chn", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idparametro_chn")]
        public int IdParametro { get; set; }

        [ColumnProperties("idvprocedimiento_chn")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmuestra_chn")]
        public int IdMuestra { get; set; }

        public double? MediaPorcentejaHumeda { get; set; }
        public double? MediaPorcentajeSeca { get; set; }
        public double? CV { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaCHNviejo> Replicas;
    }
}