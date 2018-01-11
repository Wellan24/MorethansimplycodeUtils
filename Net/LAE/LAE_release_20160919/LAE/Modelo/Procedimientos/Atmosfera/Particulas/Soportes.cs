using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaSoporte
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("particulas.soporte")]
    public class Soporte : PersistenceData
    {
        [ColumnProperties("id_soporte", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("numcodigo_soporte")]
        public int? NumCodigo { get; set; }

        [ColumnProperties("fecharecepcion_soporte")]
        public DateTime FechaRecepcion { get; set; }

        [ColumnProperties("filtro_soprte")]
        public Boolean? Filtro { get; set; }

        [ColumnProperties("tiposoporte_soporte")]
        public String TipoSoporte { get; set; }
    }
}