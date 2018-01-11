using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaProcedimientos
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("procedimientos")]
    public class Procedimiento : PersistenceData
    {
        [ColumnProperties("id_procedimiento", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("codigo_procedimiento")]
        public String Codigo { get; set; }

        [ColumnProperties("siglas_procedimiento")]
        public String Siglas { get; set; }

        [ColumnProperties("resumen_procedimiento")]
        public String Resumen { get; set; }

        [ColumnProperties("descripcion_procedimiento")]
        public String Descripcion { get; set; }

        [ColumnProperties("norma_procedimiento")]
        public String Norma { get; set; }
    }

    //public partial class Procedimiento
    //{
    //    private static Dictionary<String, Procedimiento> procedimientoDic;

    //    static Procedimiento()
    //    {
    //        procedimientoDic = PersistenceManager.SelectAll<Procedimiento>().ToDictionary(p => p.Siglas);
    //    }

    //    public static Procedimiento Of(String siglas)
    //    {
    //        return procedimientoDic[siglas];
    //    }
    //}
}
