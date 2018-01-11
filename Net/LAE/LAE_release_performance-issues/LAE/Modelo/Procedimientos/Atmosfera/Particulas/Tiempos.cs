using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaTiempos
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("particulas.tiempos")]
    public class Tiempo : PersistenceData
    {
        [ColumnProperties("id_tiempo", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("valor_tiempo")]
        public int Valor { get; set; }

        [ColumnProperties("idudsvalor_tiempo")]
        public int? Idudsvalor { get; set; }

        [ColumnProperties("idprocedimiento_tiempo")]
        public int? Idprocedimiento { get; set; }
    }
}
