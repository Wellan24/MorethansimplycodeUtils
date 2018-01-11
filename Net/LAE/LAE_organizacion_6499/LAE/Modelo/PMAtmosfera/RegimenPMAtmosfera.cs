using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaRegimenFPMAtmosfera
    {
        private static RegimenPMAtmosfera[] regimenes = null;
        public static RegimenPMAtmosfera[] GetRegimenes()
        {
            if (regimenes == null)
                regimenes = PersistenceManager.SelectAll<RegimenPMAtmosfera>().ToArray();
            return regimenes;
        }
    }

    [TableProperties("regimen_fpmatmosfera")]
    public class RegimenPMAtmosfera : PersistenceData
    {
        [ColumnProperties("id_regimenfpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_regimenfpmatmosfera")]
        public String Nombre { get; set; }
    }
}
