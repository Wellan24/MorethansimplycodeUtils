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
    public class FactoriaDurabilidad
    {
        public static MedicionPNT GetMedicion(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMedicion(idMuestra, "biomasa.finos", "idmedicion_finos");
        }

        public static Durabilidad GetParametro(int idMedicion)
        {
            Durabilidad dur = PersistenceManager.SelectByProperty<Durabilidad>("IdMedicion", idMedicion).FirstOrDefault();
            if (dur != null)
                dur.Replicas = PersistenceManager.SelectByProperty<ReplicaDurabilidad>("IdDurabilidad", dur.Id).ToList();

            return dur;
        }

        public static Durabilidad GetDefault()
        {
            int idGramos = Unidad.Of("Gramos").Id;

            Durabilidad dur = new Durabilidad();
            dur.Replicas = new List<ReplicaDurabilidad>();
            dur.Replicas.Add(new ReplicaDurabilidad() { IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 1, Valido = true });
            dur.Replicas.Add(new ReplicaDurabilidad() { IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 2, Valido = true });

            return dur;
        }
    }

    [TableProperties("biomasa.durabilidad")]
    public class Durabilidad : PersistenceData, IModelo
    {
        [ColumnProperties("id_durabilidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idvprocedimiento_durabilidad")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_durabilidad")]
        public int IdMedicion { get; set; }


        public int IdParametro { get { return 16; } } /* en la tabla ParametroProcedimiento su valor es 16*/

        public double? MediaDurabilidad { get; set; }
        public double? Dif { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaDurabilidad> Replicas;
    }
}