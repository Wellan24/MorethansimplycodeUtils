using Cartif.Logs;
using Dapper;
using LAE.Comun.Persistence;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaEnsayoPNT
    {
        public static EnsayoPNT[] GetEnsayos(int idMuestra, String tipo)
        {
            String consulta = @"SELECT id_ensayopnt Id, fechainicio_ensayopnt FechaInicio, idequipo_ensayopnt IdEquipo
                                    FROM ensayo_pnt
                                    INNER JOIN equipos ON idequipo_ensayopnt=id_equipo
                                    INNER JOIN tipo_equipo ON  idtipo_equipo=id_tipoequipo
                                    INNER JOIN biomasa.muestra_ensayo ON id_ensayopnt=idensayo_muestraensayo
                                    WHERE nombre_tipoequipo=:Tipo AND idmuestra_muestraensayo=:IdMuestra";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<EnsayoPNT>(consulta, new { Tipo = tipo, IdMuestra = idMuestra }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los ENSAYOS. Por favor, recargue la página o informa a soporte.");
                return new EnsayoPNT[0];
            }
        }
    }

    [TableProperties("ensayo_pnt")]
    public class EnsayoPNT : PersistenceData, IModelo
    {
        [ColumnProperties("id_ensayopnt", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fechainicio_ensayopnt")]
        public DateTime FechaInicio { get; set; }

        [ColumnProperties("idequipo_ensayopnt")]
        public int IdEquipo { get; set; }

        public override string ToString()
        {
            return FechaInicio.ToString("dd/MM/yyyy");
        }
    }
}
