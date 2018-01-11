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
    public class FactoriaDensidad
    {
        public static MedicionPNT GetMedicion(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMedicion(idMuestra, "biomasa.densidad", "idmedicion_densidad");
        }

        public static Densidad GetParametro(int idMedicion)
        {
            Densidad den = PersistenceManager.SelectByProperty<Densidad>("IdMedicion", idMedicion).FirstOrDefault();
            if (den != null)
                den.Replicas = PersistenceManager.SelectByProperty<ReplicaDensidad>("IdDensidad", den.Id).ToList();

            return den;
        }

        public static Densidad GetDefault()
        {
            int idGramos = Unidad.Of("Gramos").Id;

            Densidad den = new Densidad();
            den.Replicas = new List<ReplicaDensidad>();
            den.Replicas.Add(new ReplicaDensidad() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, Num = 1, Valido = true });
            den.Replicas.Add(new ReplicaDensidad() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, Num = 2, Valido = true });

            return den;
        }
    }

    [TableProperties("biomasa.densidad")]
    public class Densidad : PersistenceData, IModelo, IReplicas<ReplicaDensidad>
    {
        [ColumnProperties("id_densidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idcubo_densidad")]
        public int IdCubo { get; set; }

        [ColumnProperties("idhumedad_densidad")]
        public int? IdHumedad { get; set; }

        [ColumnProperties("idvprocedimiento_densidad")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_densidad")]
        public int IdMedicion { get; set; }


        public int IdParametro { get { return 5; } } /* en la tabla ParametroProcedimiento su valor es 5*/

        public double? MediaDensidadSeca { get; set; }
        public double? MediaDensidadHumeda { get; set; }
        public double? Dif { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaDensidad> Replicas { get; set; }
    }
}