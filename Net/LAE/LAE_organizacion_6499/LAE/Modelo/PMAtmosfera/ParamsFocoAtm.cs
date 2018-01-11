using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaParams_fpmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("params_fpmatmosfera")]
    public class ParamsFocoAtm : PersistenceData
    {
        [ColumnProperties("id_paramsfpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("numcodigo_paramsfpmatmosfera")]
        public int NumCodigo { get; set; }

        [ColumnProperties("hora_paramsfpmatmosfera")]
        public TimeSpan Hora { get; set; }

        [ColumnProperties("numhoras_paramsfpmatmosfera")]
        public int NumHoras { get; set; }

        [ColumnProperties("idparametro_paramsfpmatmosfera")]
        public int IdParametro { get; set; }

        [ColumnProperties("idfoco_paramsfpmatmosfera")]
        public int IdFoco { get; set; }
    }
}
