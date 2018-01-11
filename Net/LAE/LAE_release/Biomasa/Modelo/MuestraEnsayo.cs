using Cartif.Logs;
using Dapper;
using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Biomasa.Modelo
{
    public static class FactoriaMuestraEnsayo
    {
        public static MuestraEnsayo GetMuestra(int idEnsayo, int idMuestra)
        {
            String consulta = @"SELECT id_muestraensayo Id, ordenensayo_muestraensayo OrdenEnsayo, idhumedad_muestraensayo IdHumedad, idhumedad3_muestraensayo IdHumedad3, idensayo_muestraensayo IdEnsayo, idmuestra_muestraensayo IdMuestra
                                FROM biomasa.muestra_ensayo
                                WHERE idensayo_muestraensayo=:IdEnsayo AND idmuestra_muestraensayo=:IdMuestra";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<MuestraEnsayo>(consulta, new { IdEnsayo = idEnsayo, IdMuestra = idMuestra }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los ensayos. Por favor, recargue la página o informa a soporte.");
                return null;
            }
        }

        public static void LoadHumedad(this MuestraEnsayo muestra)
        {
            if ((muestra.IdHumedad ?? 0) != 0)
                muestra.Humedad = PersistenceManager.SelectByID<HumedadTotal>(muestra.IdHumedad).MediaHumedadTotalCalculado;

            if ((muestra.IdHumedad3 ?? 0) != 0)
                muestra.Humedad3 = PersistenceManager.SelectByID<Humedad3>(muestra.IdHumedad3).MediaHumedadTotalCalculado;
        }

        public static Boolean ExisteReplica(this MuestraEnsayo muestra)
        {
            String consulta = @"SELECT EXISTS(SELECT id_replicachn
                                    FROM biomasa.replica_CHN
                                    INNER JOIN biomasa.chn ON idchn_replicachn=id_chn
                                    WHERE idensayo_replicachn=@IdEnsayo AND idmuestra_chn=@IdMuestra)";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    return conn.Query<Boolean>(consulta, muestra).FirstOrDefault();
                }
                    
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al intentar borrar el Análisis. Por favor, recargue la página o informa a soporte.");
                return true;
            }
        }
    }

    [TableProperties("biomasa.muestra_ensayo")]
    public class MuestraEnsayo : PersistenceData, IModelo
    {
        [ColumnProperties("id_muestraensayo", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("ordenensayo_muestraensayo")]
        public int OrdenEnsayo { get; set; }

        [ColumnProperties("idhumedad_muestraensayo")]
        public int? IdHumedad { get; set; }

        [ColumnProperties("idhumedad3_muestraensayo")]
        public int? IdHumedad3 { get; set; }

        [ColumnProperties("idensayo_muestraensayo")]
        public int IdEnsayo { get; set; }

        [ColumnProperties("idmuestra_muestraensayo")]
        public int IdMuestra { get; set; }

        public double? Humedad { get; set; }

        public double? Humedad3 { get; set; }
    }
}