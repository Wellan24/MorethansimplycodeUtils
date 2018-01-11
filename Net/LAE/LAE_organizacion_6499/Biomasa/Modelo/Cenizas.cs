using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaCenizas
    {
        public static MedicionPNT GetMedicion(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMedicion(idMuestra, "biomasa.cenizas", "idmedicion_cenizas");
        }

        public static Cenizas GetParametro(int idMedicion)
        {
            Cenizas cen = PersistenceManager.SelectByProperty<Cenizas>("IdMedicion", idMedicion).FirstOrDefault();
            if (cen != null)
                cen.Replicas = PersistenceManager.SelectByProperty<ReplicaCeniza>("IdCenizas", cen.Id).ToList();

            return cen;
        }

        public static Cenizas GetDefault()
        {
            int idGramos = Unidad.Of("Gramos").Id;

            Cenizas cen = new Cenizas();
            cen.Replicas = new List<ReplicaCeniza>();
            cen.Replicas.Add(new ReplicaCeniza() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 1, Valido = true });
            cen.Replicas.Add(new ReplicaCeniza() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 2, Valido = true });

            return cen;
        }
    }

    [TableProperties("biomasa.cenizas")]
    public class Cenizas : PersistenceData, IModelo, IReplicas<ReplicaCeniza>
    {
        [ColumnProperties("id_cenizas", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idhumedad3_cenizas")]
        public int? IdHumedad3 { get; set; }

        [ColumnProperties("idhumedad_cenizas")]
        public int? IdHumedad { get; set; }

        [ColumnProperties("idvprocedimiento_cenizas")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_cenizas")]
        public int IdMedicion { get; set; }

        public int IdParametro { get { return 4; } } /* en la tabla ParametroProcedimiento su valor es 4*/

        public double? MediaCenizasSeca { get; set; }
        public double? MediaCenizasHU3 { get; set; }
        public double? MediaCenizasHUM { get; set; }
        public double? Dif { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaCeniza> Replicas { get; set; }
    }
}
