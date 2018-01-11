using Cartif.Logs;
using Dapper;
using LAE.Comun.Calculos;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaHumedad3
    {
        public static MedicionPNT[] GetMediciones(int idMuestra)
        {
            return FactoriaMedicionPNT.GetMediciones(idMuestra, "biomasa.humedad3", "idmedicion_humedad3");
        }

        public static Humedad3 GetParametro(int idMedicion)
        {
            Humedad3 hu3 = PersistenceManager.SelectByProperty<Humedad3>("IdMedicion", idMedicion).FirstOrDefault();
            if (hu3 != null)
                hu3.Replicas = PersistenceManager.SelectByProperty<ReplicaHumedad3>("IdHumedad", hu3.Id).ToList();

            return hu3;
        }

        public static Humedad3 GetDefault()
        {
            int idGramos = Unidad.Of("Gramos").Id;

            Humedad3 hum = new Humedad3();
            hum.Replicas = new List<ReplicaHumedad3>();
            hum.Replicas.Add(new ReplicaHumedad3() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 1, Valido = true });
            hum.Replicas.Add(new ReplicaHumedad3() { IdUdsM1 = idGramos, IdUdsM2 = idGramos, IdUdsM3 = idGramos, Num = 2, Valido = true });

            return hum;
        }

        public static Humedad3[] GetHumedades(int idMuestra)
        {
            String consulta = @"SELECT id_humedad3 Id, idvprocedimiento_humedad3 IdVProcedimiento, idmedicion_humedad3 IdMedicion
                                    FROM biomasa.humedad3
                                    INNER JOIN medicion_pnt ON idmedicion_humedad3 = id_medicionpnt
                                    LEFT JOIN medicion_pntcci ON id_medicionpnt = idcci_medicionpntcci
                                    WHERE idcci_medicionpntcci is null AND idmuestra_medicionpnt = :IdMuestra";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<Humedad3>(consulta, new { IdMuestra = idMuestra }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener las humedades 3. Por favor, recargue la página o informa a soporte.");
                return null;
            }
            
        }

        public static Humedad3[] GetHumedades(int[] idMuestras)
        {
            String consulta = @"SELECT id_humedad3 Id, idvprocedimiento_humedad3 IdVProcedimiento, idmedicion_humedad3 IdMedicion
                                    FROM biomasa.humedad3
                                    INNER JOIN medicion_pnt ON idmedicion_humedad3 = id_medicionpnt
                                    LEFT JOIN medicion_pntcci ON id_medicionpnt = idcci_medicionpntcci
                                    WHERE idcci_medicionpntcci is null AND idmuestra_medicionpnt = ANY(@IdMuestras)";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<Humedad3>(consulta, new { IdMuestras = idMuestras }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener las humedades 3. Por favor, recargue la página o informa a soporte.");
                return null;
            }
        }
    }

    [TableProperties("biomasa.humedad3")]
    public class Humedad3 : PersistenceData, IModelo, IReplicas<ReplicaHumedad3>
    {
        [ColumnProperties("id_humedad3", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idvprocedimiento_humedad3")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idmedicion_humedad3")]
        public int IdMedicion { get; set; }

        public int IdParametro { get { return 26; } } /* en la tabla ParametroProcedimiento su valor es 26*/

        public double? MediaHumedadTotal { get; set; }
        public double? Diferencia { get; set; }
        public bool Aceptado { get; set; }

        public List<ReplicaHumedad3> Replicas { get; set; }


        public double? MediaHumedadTotalCalculado
        {
            get
            {
                List<ReplicaHumedad3> replicas = PersistenceManager.SelectByProperty<ReplicaHumedad3>("IdHumedad", Id).Where(r=>r.Valido==true).ToList();
                foreach (ReplicaHumedad3 replica in replicas)
                {
                    Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                    Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                    Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                    if (m1 == null || m2 == null || m3 == null)
                        return 0;
                    replica.HumedadTotal = Calcular.Humedad3_8_11(m1, m2, m3)?.Value;
                }
                Valor[] valoresHumedad = replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.HumedadTotal, "%")).ToArray();
                return Calcular.Promedio(valoresHumedad).Value;
            }
        }

        public override string ToString()
        {
            return String.Format("HU3: {0:#.##}", MediaHumedadTotalCalculado);
        }
    }
}