using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaRecepcionBiomasa
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("recepcion_biomasa")]
    public class RecepcionBiomasa : PersistenceData, IModelo
    {
        [ColumnProperties("id_recepcionbiomasa", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fecharecepcion_recepcionbiomasa")]
        public DateTime? FechaRecepcion { get; set; }

        [ColumnProperties("observaciones_recepcionbiomasa")]
        public String Observaciones { get; set; }

        [ColumnProperties("idtecnico_recepcionbiomasa")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idtrabajo_recepcionbiomasa")]
        public int IdTrabajo { get; set; }
    }
}