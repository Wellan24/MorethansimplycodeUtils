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

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaAlcance
    {
        public static double? GetRango(int idVProcedimiento, int idParametro, bool superior)
        {
            StringBuilder consulta;
            if (superior)
                consulta = new StringBuilder("SELECT rangosuperior_alcance ");
            else
                consulta = new StringBuilder("SELECT rangoinferior_alcance ");
            consulta.Append(@"
                                    FROM alcance
                                    WHERE idvprocedimiento_alcance = :IdVer
                                        AND idparametro_alcance = :IdParam");

            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<double?>(consulta.ToString(), new { IdVer = idVProcedimiento, IdParam = idParametro }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener el rango de Alcance. Por favor, recargue la página o informa a soporte.");
                return 0;
            }
        }
    }

    [TableProperties("alcance")]
    public class Alcance : PersistenceData
    {
        [ColumnProperties("id_alcance", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("rangoinferior_alcance")]
        public decimal? RangoInferior { get; set; }

        [ColumnProperties("rangosuperior_alcance")]
        public decimal? RangoSuperior { get; set; }

        [ColumnProperties("idvprocedimiento_alcance")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idparametro_alcance")]
        public int IdParametro { get; set; }
    }
}