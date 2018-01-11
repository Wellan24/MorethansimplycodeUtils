using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaParametroProcedimiento
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("parametro_procedimiento")]
    public class ParametroProcedimiento : PersistenceData
    {
        [ColumnProperties("id_parametroprocedimiento", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_parametroprocedimiento")]
        public String Nombre { get; set; }

        [ColumnProperties("idprocedimiento_parametroprocedimiento")]
        public int IdProcedimiento { get; set; }
    }
}