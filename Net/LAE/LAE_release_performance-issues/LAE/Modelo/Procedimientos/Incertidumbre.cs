using Cartif.Logs;
using Dapper;
using LAE.Calculos;
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
    public class FactoriaIncertidumbre
    {
        public static RegresionLineal GetIncertidumbre(int idVProcedimiento, int idParametro, double value)
        {
            String consulta = @"SELECT fijo_incertidumbre Interseccion, pendiente_incertidumbre Pendiente
                                    FROM incertidumbre
                                    WHERE idvprocedimiento_incertidumbre = :IdVer
                                        AND idparametro_incertidumbre = :IdParam
                                        AND (limiteinferior_incertidumbre is null OR limiteinferior_incertidumbre <= :Value)
                                        AND (limitesuperior_incertidumbre is null OR limitesuperior_incertidumbre > :Value)";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<RegresionLineal>(consulta.ToString(), new { IdVer = idVProcedimiento, IdParam = idParametro, Value = value }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener la Incertidumbre. Por favor, recargue la página o informa a soporte.");
                return null;
            }
        }
    }

    [TableProperties("incertidumbre")]
    public class Incertidumbre : PersistenceData
    {
        [ColumnProperties("id_incertidumbre", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("limiteinferior_incertidumbre")]
        public decimal? LimiteInferior { get; set; }

        [ColumnProperties("limitesuperior_incertidumbre")]
        public decimal? LimiteSuperior { get; set; }

        [ColumnProperties("fijo_incertidumbre")]
        public decimal? Fijo { get; set; }

        [ColumnProperties("pendiente_incertidumbre")]
        public decimal? Pendiente { get; set; }

        [ColumnProperties("idvprocedimiento_incertidumbre")]
        public int IdVProcedimiento { get; set; }

        [ColumnProperties("idparametro_incertidumbre")]
        public int IdParametro { get; set; }
    }
}