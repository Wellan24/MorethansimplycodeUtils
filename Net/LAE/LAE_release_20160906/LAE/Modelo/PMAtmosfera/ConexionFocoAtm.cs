using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaConexion_fpmatmosfera
    {
        // TODO Rellenar esto con Selects necesarias.
    }

    [TableProperties("conexion_fpmatmosfera")]
    public class ConexionFocoAtm : PersistenceData
    {
        [ColumnProperties("id_conexionfpmatmosfera", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_conexionfpmatmosfera")]
        public int NumConexion { get; set; }

        [ColumnProperties("idfoco_conexionfpmatmosfera")]
        public int IdFoco { get; set; }

        public ObservableCollection<PuntoConexFocoAtmosfera> PuntosMuestreo;

    }
}
