using Cartif.Logs;
using Dapper;
using LAE.Calculos;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Modelo
{
    public class FactoriaHumedadTotal
    {

        //TODO borrar metodo
        public static MedicionPNT GetMedicion(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMedicion(idMuestra, "biomasa.humedad_total", "idmedicion_humedadtotal");
        }

        public static MedicionPNT[] GetMediciones(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMediciones(idMuestra, "biomasa.humedad_total", "idmedicion_humedadtotal", true);
        }

        public static HumedadTotal GetParametro(int idMedicion)
        {
            HumedadTotal hum = PersistenceManager.SelectByProperty<HumedadTotal>("IdMedicion", idMedicion).FirstOrDefault();

            if (hum != null)
                hum.Replicas = PersistenceManager.SelectByProperty<ReplicaHumedadTotal>("IdHumedad", hum.Id).ToList();

            return hum;
        }

        public static HumedadTotal GetDefault()
        {
            int idGramos = Unidad.Of("Gramos").Id;

            HumedadTotal hum = new HumedadTotal() { IdUdsM4 = idGramos, IdUdsM5 = idGramos };
            hum.Replicas = new List<ReplicaHumedadTotal>();
            hum.Replicas.Add(new ReplicaHumedadTotal() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 1, Valido = true });
            hum.Replicas.Add(new ReplicaHumedadTotal() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 2, Valido = true });

            return hum;
        }

        public static HumedadTotal[] GetHumedades(int idMuestra)
        {
            String consulta = @"SELECT id_humedadtotal Id, m4_humedadtotal M4, idudsm4_humedadtotal IdUdsM4, m5_humedadtotal M5, idudsm5_humedadtotal IdUdsM5, idvprocedimiento_humedadtotal IdVProcedimiento, idmedicion_humedadtotal IdMedicion
                                    FROM biomasa.humedad_total
                                    INNER JOIN medicion_pnt ON idmedicion_humedadtotal = id_medicionpnt
                                    LEFT JOIN medicion_pntcci ON id_medicionpnt = idcci_medicionpntcci
                                    WHERE idcci_medicionpntcci is null AND idmuestra_medicionpnt = :IdMuestra";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<HumedadTotal>(consulta, new { IdMuestra = idMuestra }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener las humedades totales. Por favor, recargue la página o informa a soporte.");
                return null;
            }

        }
    }

    [TableProperties("biomasa.humedad_total")]
    public class HumedadTotal : PersistenceData, IModelo, IReplicas<ReplicaHumedadTotal>
    {
        [ColumnProperties("id_humedadtotal", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("m4_humedadtotal")]
        public double? M4 { get; set; }

        [ColumnProperties("idudsm4_humedadtotal")]
        public int? IdUdsM4 { get; set; }

        [ColumnProperties("m5_humedadtotal")]
        public double? M5 { get; set; }

        [ColumnProperties("idudsm5_humedadtotal")]
        public int? IdUdsM5 { get; set; }

        [ColumnProperties("idvprocedimiento_humedadtotal")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_humedadtotal")]
        public int IdMedicion { get; set; }

        public int IdParametro { get { return 1; } } /* en la tabla ParametroProcedimiento su valor es 1*/

        public double? MediaHumedadTotal { get; set; }
        public double? CV { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaHumedadTotal> Replicas {get; set; }

        public double? MediaHumedadTotalCalculado
        {
            get
            {
                if (IdUdsM4 == null || IdUdsM5 == null || M4==null || M5==null)
                    return 0;
                Valor m4 = Valor.Of(M4, IdUdsM4 ?? 0);
                Valor m5 = Valor.Of(M5, IdUdsM5 ?? 0);

                List<ReplicaHumedadTotal> replicas = PersistenceManager.SelectByProperty<ReplicaHumedadTotal>("IdHumedad", Id).Where(r => r.Valido == true).ToList();
                foreach (ReplicaHumedadTotal replica in replicas)
                {
                    Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                    Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                    Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                    if (m1 == null || m2 == null || m3 == null)
                        return 0;
                    replica.HumedadTotal = Calcular.HumedadTotal_8_1(m1, m2, m3, m4, m5)?.Value;
                }
                Valor[] valoresHumedad = replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.HumedadTotal, "%")).ToArray();
                return Calcular.Promedio(valoresHumedad).Value;
            }
        }

        public override string ToString()
        {
            return String.Format("HUM: {0:#.##}", MediaHumedadTotalCalculado);
        }
    }
}