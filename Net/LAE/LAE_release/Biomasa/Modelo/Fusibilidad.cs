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
    public class FactoriaFusibilidad
    {
        public static Fusibilidad GetParametro(int idMuestra)
        {
            Fusibilidad fus = PersistenceManager.SelectByProperty<Fusibilidad>("IdMuestra", idMuestra).FirstOrDefault();
            if (fus != null)
                fus.Replicas = PersistenceManager.SelectByProperty<ReplicaFusibilidad>("IdFusibilidad", fus.Id).ToList();
            return fus;
        }

        public static Fusibilidad GetDefault(int idMuestra)
        {
            int idTemperatura = Unidad.Of("ºC").Id;
            Fusibilidad fus = new Fusibilidad() { IdMuestra = idMuestra };
            fus.Replicas = new List<ReplicaFusibilidad>();
            fus.Replicas.Add(new ReplicaFusibilidad() { IdUdsTemperaturaSst = idTemperatura, IdUdsTemperaturaDT = idTemperatura, IdUdsTemperaturaHT = idTemperatura, IdUdsTemperaturaFT = idTemperatura, Num = 1, Valido = true });
            fus.Replicas.Add(new ReplicaFusibilidad() { IdUdsTemperaturaSst = idTemperatura, IdUdsTemperaturaDT = idTemperatura, IdUdsTemperaturaHT = idTemperatura, IdUdsTemperaturaFT = idTemperatura, Num = 2, Valido = true });

            return fus;
        }
    }

    [TableProperties("biomasa.fusibilidad")]
    public class Fusibilidad : PersistenceData, IModelo, IReplicas<ReplicaFusibilidad>
    {
        [ColumnProperties("id_fusibilidad", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idvprocedimiento_fusibilidad")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmuestra_fusibilidad")]
        public int IdMuestra { get; set; }

        [ColumnProperties("finalizado_fusibilidad")]
        public Boolean Finalizado { get; set; }

        public ValoresFusibilidad this[String key] => Valores[key];
        private CartifDictionary<String, ValoresFusibilidad> Valores = new CartifDictionary<string, ValoresFusibilidad>()
        {
            ["SST"] = new ValoresFusibilidad(10),
            ["DT"] = new ValoresFusibilidad(11),
            ["HT"] = new ValoresFusibilidad(12),
            ["FT"] = new ValoresFusibilidad(13),
        };

        public List<ReplicaFusibilidad> Replicas { get; set; }
    }

    public class ValoresFusibilidad
    {
        public int IdParametro { get; }

        public double? MediaTemperatura { get; set; }

        public double? CV { get; set; }

        public bool Aceptado { get; set; }

        public ValoresFusibilidad(int idParametro)
        {
            IdParametro = idParametro;
        }
    }
}
