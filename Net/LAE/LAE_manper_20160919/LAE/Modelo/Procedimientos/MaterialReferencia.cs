using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMaterialesReferencia
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("materiales_referencia")]
    public class MaterialReferencia : PersistenceData
    {
        [ColumnProperties("id_materialreferencia", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("codigo_materialreferencia")]
        public int? Codigo { get; set; }

        [ColumnProperties("certificado_materialreferencia")]
        public Boolean? Certificado { get; set; }

        [ColumnProperties("usable_materialreferencia")]
        public Boolean? Usable { get; set; }

        [ColumnProperties("marca_materialreferencia")]
        public String Marca { get; set; }

        [ColumnProperties("fecharecepcion__materialreferencia")]
        public DateTime FechaRecepcion { get; set; }

        [ColumnProperties("fechacaducidad_materialreferencia")]
        public DateTime FechaCaducidad { get; set; }

        [ColumnProperties("fechausolimite_materialreferencia")]
        public DateTime? FechaUsoLimite { get; set; }

        [ColumnProperties("idtecnico_materialreferencia")]
        public int IdTecnico { get; set; }

        [ColumnProperties("conservacion_materialreferencia")]
        public String Conservacion { get; set; }
    }
}
