using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaParamsLaboratorioMuestraAgua
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("paramslaboratorio_muestraagua")]
    public class ParamsLaboratorioMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_paramslaboratoriomuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idparametro_paramslaboratoriomuestraagua")]
        public int IdParametro { get; set; }

        [ColumnProperties("idmuestraagua_paramslaboratoriomuestraagua")]
        public int IdMuestra { get; set; }

        public override bool Equals(object obj)
        {
            return Object.ReferenceEquals(this, obj);
        }
    }
}
