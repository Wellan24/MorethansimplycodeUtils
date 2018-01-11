using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaSolicitudesAceptacion
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("solicitudes_aceptacion")]
    public class SolicitudesAceptacion : PersistenceData
    {
        [ColumnProperties("id_solicitudaceptacion", IsId =true, IsAutonumeric =true)]
        public int Id { get; set; }

        [ColumnProperties("documentofirma_solicitudaceptacion")]
        public byte[] DocumentoFirma { get; set; }

        [ColumnProperties("codigo_solicitudaceptacion")]
        public String Codigo { get; set; }

        [ColumnProperties("observaciones_solicitudaceptacion")]
        public String Observaciones { get; set; }

        [ColumnProperties("firmadocliente_solicitudaceptacion")]
        public bool FirmadoCliente { get; set; }

        [ColumnProperties("firmadolae_solicitudaceptacion")]
        public bool FirmadoLae { get; set; }

        [ColumnProperties("fechafirmacliente_solicitudaceptacion")]
        public DateTime FechaFirmaCliente { get; set; }

        [ColumnProperties("fechafirmalae_solicitudaceptacion")]
        public DateTime FechaFirmaLae { get; set; }

        [ColumnProperties("idcontacto_solicitudaceptacion")]
        public int IdContacto { get; set; }

        [ColumnProperties("idtecnico_solicitudaceptacion")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idrevisionoferta_solicitudaceptacion")]
        public int IdRevisionOferta { get; set; }
    }
}
