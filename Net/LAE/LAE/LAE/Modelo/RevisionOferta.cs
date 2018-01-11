using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaRevisionesOferta
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("revisiones_oferta")]
    public class RevisionOferta : PersistenceData
    {
        [ColumnProperties("id_revisionoferta", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_revisionoferta")]
        public int? Num { get; set; }

        [ColumnProperties("objeto_revisionoferta")]
        public String Objeto { get; set; }

        [ColumnProperties("fechaemision_revisionoferta")]
        public DateTime? FechaEmision { get; set; }

        [ColumnProperties("importe_revisionoferta")]
        public int Importe { get; set; }

        [ColumnProperties("requieretomamuestra_revisionoferta")]
        public bool? RequiereTomaMuestra { get; set; }

        [ColumnProperties("lugarmuestra_revisionoferta")]
        public String LugarMuestra { get; set; }

        [ColumnProperties("numpuntosmuestreo_revisionoferta")]
        public int? NumPuntosMuestreo { get; set; }

        [ColumnProperties("trabajopuntual_revisionoferta")]
        public bool? TrabajoPuntual { get; set; }

        [ColumnProperties("frecuencia_revisionoferta")]
        public String Frecuencia { get; set; }

        [ColumnProperties("plazorealizacion_revisionoferta")]
        public String PlazoRealizacion { get; set; }

        [ColumnProperties("idoferta_revisionoferta")]
        public int? IdOferta { get; set; }

        [ColumnProperties("idtecnico_revisionoferta")]
        public int IdTecnico { get; set; }

    }
}
