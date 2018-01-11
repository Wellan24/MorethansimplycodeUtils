using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaMuestreoFPMAtmosfera
    {
        private static MuestreoFocoAtm[] muestreos = null;
        public static MuestreoFocoAtm[] GetMuestreos()
        {
            if (muestreos == null)
                muestreos = PersistenceManager.SelectAll<MuestreoFocoAtm>().ToArray();
            return muestreos;
        }
    }

    [TableProperties("muestreo_fpmatmosfera")]
    public class MuestreoFocoAtm : PersistenceData
    {
        [ColumnProperties("id_muestreofpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_muestreofpmatmosfera")]
        public String Nombre { get; set; }
    }
}
