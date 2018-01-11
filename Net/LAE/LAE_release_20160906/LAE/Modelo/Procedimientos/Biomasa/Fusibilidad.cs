using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaFusibilidad
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("biomasa.fusibilidad")]
    public class Fusibilidad : PersistenceData
    {
        [ColumnProperties("id_fusibilidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idparametro_fusibilidad")]
        public int IdParametro { get; set; }

        [ColumnProperties("idvprocedimiento_fusibilidad")]
        public int IdVProcedimiento { get; set; }

        public double? MediaTemperatura { get; set; }
        public double? CV { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaFusibilidad> Replicas;
    }
}