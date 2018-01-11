using Cartif.Collections;
using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaCHN
    {
        // TODO Rellenar esto con Selects necesarias.
        public static Chn GetParametro(int idMuestra)
        {
            Chn chn = PersistenceManager.SelectByProperty<Chn>("IdMuestra", idMuestra).FirstOrDefault();
            if (chn != null)
                chn.Replicas = PersistenceManager.SelectByProperty<ReplicaChn>("IdCHN", chn.Id).ToList();
            return chn;
        }

        public static Chn GetDefault(int idMuestra)
        {
            int idPorcentaje = Unidad.Of("%").Id;
            Chn chn = new Chn() { IdMuestra = idMuestra };
            chn.Replicas = new List<ReplicaChn>();
            chn.Replicas.Add(new ReplicaChn() { IdUdsPorcentajeC = idPorcentaje, IdUdsPorcentajeH = idPorcentaje, IdUdsPorcentajeN = idPorcentaje, Num = 1, Valido = true });
            chn.Replicas.Add(new ReplicaChn() { IdUdsPorcentajeC = idPorcentaje, IdUdsPorcentajeH = idPorcentaje, IdUdsPorcentajeN = idPorcentaje, Num = 2, Valido = true });

            return chn;
        }
    }

    [TableProperties("biomasa.chn")]
    public class Chn : PersistenceData, IModelo, IReplicas<ReplicaChn>
    {
        [ColumnProperties("id_chn", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idhumedad3_chn")]
        public int? IdHumedad3 { get; set; }

        [ColumnProperties("idvprocedimiento_chn")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmuestra_chn")]
        public int IdMuestra { get; set; }

        public ValoresCHN this[String key] => Valores[key];
        private CartifDictionary<String, ValoresCHN> Valores = new CartifDictionary<String, ValoresCHN>()
        {
            ["C"] = new ValoresCHN(6), /* en la tabla ParametroProcedimiento su valor es 6*/
            ["H"] = new ValoresCHN(7), /* en la tabla ParametroProcedimiento su valor es 7*/
            ["N"] = new ValoresCHN(8)  /* en la tabla ParametroProcedimiento su valor es 8*/
        };

        public List<ReplicaChn> Replicas { get; set; }
    }

    public class ValoresCHN
    {
        public int IdParametro { get; }

        public double? MediaPorcentajeHumeda { get; set; }

        public double? MediaPorcentajeSeca { get; set; }

        public double? CV { get; set; }

        public bool Aceptado { get; set; }

        public ValoresCHN(int idParametro)
        {
            IdParametro = idParametro;
        }
    }
}