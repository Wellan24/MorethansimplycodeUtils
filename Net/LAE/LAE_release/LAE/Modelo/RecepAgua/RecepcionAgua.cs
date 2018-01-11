using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaRecepcion_agua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("recepcion_agua")]
    public class RecepcionAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_recepcionagua", IsId =true,IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("fecharecepcion_recepcionagua")]
        public DateTime? FechaRecepcion { get; set; }

        [ColumnProperties("fechacaducidad_recepcionagua")]
        public DateTime? FechaCaducidad { get; set; }

        [ColumnProperties("observaciones_recepcionagua")]
        public String Observaciones { get; set; }

        [ColumnProperties("idtecnico_recepcionagua")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idtrabajo_recepcionagua")]
        public int IdTrabajo { get; set; }
    }
}
