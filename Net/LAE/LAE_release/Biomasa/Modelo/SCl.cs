using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaSCl
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.SCl")]
    public class SCl : PersistenceData
    {
        [ColumnProperties("id_scl", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("blanco_scl")]
        public double? Blanco { get; set; }

        [ColumnProperties("idudsblanco_scl")]
        public int? IdUdsBlanco { get; set; }

        [ColumnProperties("idparametro_scl")]
        public int IdParametro { get; set; }

        [ColumnProperties("idvprocedimiento_scl")]
        public int IdVProcedimiento { get; set; }

        public double? MediaPorcentejaHumeda { get; set; }
        public double? MediaPorcentajeSeca { get; set; }
        public double? CV { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaSCl> Replicas;
    }
}