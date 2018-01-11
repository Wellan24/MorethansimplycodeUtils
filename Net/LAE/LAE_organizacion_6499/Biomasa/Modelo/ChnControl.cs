using Cartif.Collections;
using Cartif.Logs;
using Dapper;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LAE.Comun.Modelo;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaCHNControl
    {
        // TODO Rellenar esto con Selects necesarias.
        public static ChnControl[] GetControles(int idEnsayo)
        {
            ReplicaChnControl[] replicas = PersistenceManager.SelectByProperty<ReplicaChnControl>("IdEnsayo", idEnsayo).ToArray();

            var c = from replica in replicas
                    group replica by replica.IdCHN into g
                    select new { control = PersistenceManager.SelectByID<ChnControl>(g.Key), replicas = g.ToList() };

            List<ChnControl> lista = new List<ChnControl>();
            foreach (var item in c)
            {
                item.control.Replicas = item.replicas;
                lista.Add(item.control);
            }

            return lista.ToArray();

        }

        public static ChnControl GetDefault(int idEnsayo)
        {
            int idPorcentaje = Unidad.Of("%").Id;

            ChnControl chn = new ChnControl() { IdEnsayo = idEnsayo };
            chn.Replicas = new List<ReplicaChnControl>();
            chn.Replicas.Add(new ReplicaChnControl() { IdUdsPorcentajeC = idPorcentaje, IdUdsPorcentajeH = idPorcentaje, IdUdsPorcentajeN = idPorcentaje, Num = 1, Valido = true });
            chn.Replicas.Add(new ReplicaChnControl() { IdUdsPorcentajeC = idPorcentaje, IdUdsPorcentajeH = idPorcentaje, IdUdsPorcentajeN = idPorcentaje, Num = 2, Valido = true });

            return chn;
        }
    }

    [TableProperties("biomasa.chn_control")]
    public class ChnControl : PersistenceData, IModelo
    {
        [ColumnProperties("id_chncontrol", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("ordenensayo_chncontrol")]
        public int OrdenEnsayo { get; set; }

        [ColumnProperties("idtecnico_chncontrol")]
        public int IdTecnico { get; set; }

        [ColumnProperties("idvprocedimiento_chncontrol")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("observaciones_chncontrol")]
        public String Observaciones { get; set; }

        [ColumnProperties("idmaterialreferencia_chncontrol")]
        public int IdMaterialReferencia { get; set; }

        [ColumnProperties("idensayo_chncontrol")]
        public int IdEnsayo { get; set; }

        public ValoresCHNcontrol this[String key] => Valores[key];
        private CartifDictionary<String, ValoresCHNcontrol> Valores = new CartifDictionary<string, ValoresCHNcontrol>()
        {
            ["C"] = new ValoresCHNcontrol(6, 27), /* en la tabla ParametroProcedimiento su valor es C=6 y C_CCI=27 */
            ["H"] = new ValoresCHNcontrol(7, 28), /* en la tabla ParametroProcedimiento su valor es H=7 y C_CCI=28 */
            ["N"] = new ValoresCHNcontrol(8, 29)  /* en la tabla ParametroProcedimiento su valor es N=8 y C_CCI=29 */
        };

        public List<ReplicaChnControl> Replicas { get; set; }
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