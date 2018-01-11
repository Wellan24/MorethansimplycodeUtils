using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaFinos
    {
        public static Finos GetParametro(int idMedicion)
        {
            Finos fin = PersistenceManager.SelectByProperty<Finos>("IdMedicion", idMedicion).FirstOrDefault();
            if (fin != null)
                fin.Replicas = PersistenceManager.SelectByProperty<ReplicaFinos>("IdFinos", fin.Id).ToList();

            return fin;
        }

        public static Finos GetDefault()
        {
            int idGramos = Unidad.Of("Gramos").Id;

            Finos fin = new Finos();
            fin.Replicas = new List<ReplicaFinos>();
            fin.Replicas.Add(new ReplicaFinos() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, Num = 1, Valido = true });
            fin.Replicas.Add(new ReplicaFinos() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, Num = 2, Valido = true });

            return fin;
        }
    }

    [TableProperties("biomasa.finos")]
    public class Finos : PersistenceData, IModelo
    {
        [ColumnProperties("id_finos", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idvprocedimiento_finos")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_finos")]
        public int IdMedicion { get; set; }


        public int IdParametro { get { return 17; } } /* en la tabla ParametroProcedimiento su valor es 17*/

        public double? MediaFinos { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaFinos> Replicas;
    }
}