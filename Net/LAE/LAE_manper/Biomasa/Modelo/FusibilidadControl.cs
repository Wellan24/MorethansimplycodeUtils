using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaFusibilidadControl
    {
        // TODO Rellenar esto con Selects necesarias.
        public static FusibilidadControl GetDefault(int idEnsayo)
        {
            int idTemperatura = Unidad.Of("ºC").Id;

            FusibilidadControl fus = new FusibilidadControl() { IdEnsayo = idEnsayo };
            fus.Replica = new ReplicaFusibilidadControl() { IdUdsTemperatura = idTemperatura };

            return fus;
        }
    }

    [TableProperties("biomasa.fusibilidad_control")]
    public class FusibilidadControl : PersistenceData, IModelo
    {
        [ColumnProperties("id_fusibilidadcontrol", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("ordenensayo_fusibilidadcontrol")]
        public int OrdenEnsayo { get; set; }

        [ColumnProperties("idtecnico_fusibilidadcontrol")]
        public int IdTecnico { get; set; }

        [ColumnProperties("observaciones_fusibilidadcontrol")]
        public String Observaciones { get; set; }

        [ColumnProperties("idvprocedimiento_fusibilidadcontrol")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmaterialreferencia_fusibilidadcontrol")]
        public int IdMaterialReferencia { get; set; }

        [ColumnProperties("idensayo_fusibilidadcontrol")]
        public int IdEnsayo { get; set; }

        public int IdParametro { get { return 30; } } /* en la tabla ParametroProcedimiento su valor es 4*/

        public double? Dif { get; set; }

        public bool Aceptado { get; set; }

        public ReplicaFusibilidadControl Replica { get; set; }
    }
}