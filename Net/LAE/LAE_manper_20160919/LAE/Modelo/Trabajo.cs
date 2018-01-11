using Cartif.Logs;
using Dapper;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Modelo
{
    public class FactoriaTrabajos
    {

    }

    [TableProperties("trabajos")]
    public class Trabajo : PersistenceData
    {
        [ColumnProperties("id_trabajo", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("documentofirma_trabajo")]
        public byte[] DocumentoFirma { get; set; }

        [ColumnProperties("codigo_trabajo")]
        public int Codigo { get; set; }

        [ColumnProperties("observaciones_trabajo")]
        public String Observaciones { get; set; }

        [ColumnProperties("firmadocliente_trabajo")]
        public bool FirmadoCliente { get; set; }

        [ColumnProperties("firmadolae_trabajo")]
        public bool FirmadoLae { get; set; }

        [ColumnProperties("fechafirmacliente_trabajo")]
        public DateTime FechaFirmaCliente { get; set; }

        [ColumnProperties("fechafirmalae_trabajo")]
        public DateTime FechaFirmaLae { get; set; }

        [ColumnProperties("idcontacto_trabajo")]
        public int IdContacto { get; set; }

        [ColumnProperties("idtecnico_trabajo")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idoferta_trabajo")]
        public int IdOferta { get; set; }

        public string NumCodigo
        {
            get { return String.Format("{0:0#}", Codigo); }
            set { }
        }
    }
}
