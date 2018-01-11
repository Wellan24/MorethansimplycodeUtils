using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaMedicionPNTcci
    {
        public static MedicionPNT GetMedicion(int id)
        {
            MedicionPNTcci medCCI = PersistenceManager.SelectByID<MedicionPNTcci>(id);
            return (medCCI == null) ? null : PersistenceManager.SelectByID<MedicionPNT>(medCCI.IdCCI);
        }
    }

    [TableProperties("medicion_pntcci")]
    public class MedicionPNTcci : PersistenceData, IModelo
    {
        [ColumnProperties("id_medicionpntcci", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idcci_medicionpntcci")]
        public int IdCCI { get; set; }

    }
}