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
    public class FactoriaAceptacion
    {
        public static double? GetRango(int idVProcedimiento, int idParametro, bool superior, double? limite=null)
        {
            StringBuilder consulta;
            if (superior)
                consulta = new StringBuilder("SELECT rangosuperior_aceptacion ");
            else
                consulta = new StringBuilder("SELECT rangoinferior_aceptacion ");
            consulta.Append(@"
                                    FROM aceptacion
                                    WHERE idvprocedimiento_aceptacion = :IdVer
                                        AND idparametro_aceptacion = :IdParam");
            if (limite != null)
                consulta.Append(@"
                                        AND (limiteinferior_aceptacion is null OR limiteinferior_aceptacion <= :Limite)
                                        AND (limitesuperior_aceptacion is null OR limitesuperior_aceptacion > :Limite)");

            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<double?>(consulta.ToString(), new { IdVer = idVProcedimiento, IdParam = idParametro, Limite=limite }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener el rango de Aceptación. Por favor, recargue la página o informa a soporte.");
                return 0;
            }
        }
    }

    [TableProperties("aceptacion")]
    public class Aceptacion : PersistenceData
    {
        [ColumnProperties("id_aceptacion", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("limiteinferior_aceptacion")]
        public decimal? LimiteInferior { get; set; }

        [ColumnProperties("limitesuperior_aceptacion")]
        public decimal? LimiteSuperior { get; set; }

        [ColumnProperties("rangoinferior_aceptacion")]
        public decimal? RangoInferior { get; set; }

        [ColumnProperties("rangosuperior_aceptacion")]
        public decimal? RangoSuperior { get; set; }

        [ColumnProperties("idvprocedimiento__aceptacion")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idparametro_aceptacion")]
        public int IdParametro { get; set; }
    }
}