using Cartif.Logs;
using Dapper;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Modelo
{
    public class FactoriaVersionProcedimiento
    {
        public static VersionProcedimiento[] GetVersion(int idParametro)
        {
            String consulta = @"SELECT id_versionprocedimiento Id, num_versionprocedimiento Num, fecha_versionprocedimiento Fecha, idprocedimiento_versionprocedimiento IdProcedimiento
                                    FROM version_procedimiento
                                    INNER JOIN procedimientos ON idprocedimiento_versionprocedimiento = id_procedimiento
                                    INNER JOIN parametro_procedimiento ON id_procedimiento = idprocedimiento_parametroprocedimiento
                                    WHERE id_parametroprocedimiento = :IdParametro
                                    ORDER BY id_versionprocedimiento DESC";

            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<VersionProcedimiento>(consulta, new { IdParametro = idParametro }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener la versión. Por favor, recargue la página o informa a soporte.");
                return null;
            }

        }
    }

    [TableProperties("version_procedimiento")]
    public class VersionProcedimiento : PersistenceData
    {
        [ColumnProperties("id_versionprocedimiento", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_versionprocedimiento")]
        public int? Num { get; set; }

        [ColumnProperties("fecha_versionprocedimiento")]
        public DateTime Fecha { get; set; }

        [ColumnProperties("idprocedimiento_versionprocedimiento")]
        public int IdProcedimiento { get; set; }

        public override string ToString()
        {
            return "V." + Num;
        }
    }
}