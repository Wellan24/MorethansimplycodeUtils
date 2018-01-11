using Cartif.Collections;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaChnControl
    {
        // TODO Rellenar esto con Selects necesarias.
        public static CHNcontrol[] GetControles(int idEnsayo)
        {
            ReplicaCHNcontrol[] replicas = PersistenceManager.SelectByProperty<ReplicaCHNcontrol>("IdEnsayo", idEnsayo).ToArray();

            var c = from replica in replicas
                    group replica by replica.IdCHN into g
                    select new { control = PersistenceManager.SelectByID<CHNcontrol>(g.Key), replicas = g.ToList() };

            List<CHNcontrol> lista = new List<CHNcontrol>();
            foreach (var item in c)
            {
                item.control.Replicas = item.replicas;
                lista.Add(item.control);
            }

            return lista.ToArray();

        }

        public static CHNcontrol GetDefault(int idEnsayo)
        {
            int idPorcentaje = Unidad.Of("%").Id;

            CHNcontrol chn = new CHNcontrol();
            chn.Replicas = new List<ReplicaCHNcontrol>();
            chn.Replicas.Add(new ReplicaCHNcontrol() { IdEnsayo = idEnsayo, IdUdsPorcentajeC = idPorcentaje, IdUdsPorcentajeH = idPorcentaje, IdUdsPorcentajeN = idPorcentaje, Num = 1, Valido = true });
            chn.Replicas.Add(new ReplicaCHNcontrol() { IdEnsayo = idEnsayo, IdUdsPorcentajeC = idPorcentaje, IdUdsPorcentajeH = idPorcentaje, IdUdsPorcentajeN = idPorcentaje, Num = 2, Valido = true });

            return chn;
        }
    }

    [TableProperties("biomasa.chn_control")]
    public class CHNcontrol : PersistenceData
    {
        [ColumnProperties("id_chncontrol", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idvprocedimiento_chncontrol")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmaterialreferencia_chncontrol")]
        public int IdMaterialReferencia { get; set; }

        public ValoresCHNcontrol this[String key] => Valores[key];
        private CartifDictionary<String, ValoresCHNcontrol> Valores = new CartifDictionary<string, ValoresCHNcontrol>()
        {
            ["C"] = new ValoresCHNcontrol(6, 27), /* en la tabla ParametroProcedimiento su valor es C=6 y C_CCI=27 */
            ["H"] = new ValoresCHNcontrol(7, 28), /* en la tabla ParametroProcedimiento su valor es H=7 y C_CCI=28 */
            ["N"] = new ValoresCHNcontrol(8, 29)  /* en la tabla ParametroProcedimiento su valor es N=8 y C_CCI=29 */
        };

        public List<ReplicaCHNcontrol> Replicas { get; set; }
    }

    public class ValoresCHNcontrol
    {
        public int IdParametro { get; }

        public int IdParametro_CCI { get; }

        public double? CV { get; set; }

        public bool AceptadoCV { get; set; }

        public double? Er { get; set; }

        public bool AceptadoEr { get; set; }

        public ValoresCHNcontrol(int idParametro, int idParametro_CCI)
        {
            IdParametro = idParametro;
            IdParametro_CCI = idParametro_CCI;
        }

    }
}