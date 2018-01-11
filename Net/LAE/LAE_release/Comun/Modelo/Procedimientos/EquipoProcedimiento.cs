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
    public class FactoriaEquipoProcedimiento
    {
        // TODO Rellenar esto con Selects necesarias.
        public static List<Equipo> GetEquipos(int idParametro)
        {
            String consulta = @"SELECT id_equipo Id, nombre_equipo Nombre, idtipo_equipo IdTipo,  predefinido_equipoprocedimiento Predefinido
                                    FROM equipos
                                    INNER JOIN equipo_procedimiento ON idequipo_equipoprocedimiento = id_equipo
	                                INNER JOIN procedimientos ON  id_procedimiento = idprocedimiento_equipoprocedimiento
	                                INNER JOIN parametro_procedimiento ON id_procedimiento = idprocedimiento_parametroprocedimiento
	                                WHERE id_parametroprocedimiento=:IdParametro";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<Equipo>(consulta, new { IdParametro = idParametro }).ToList();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los equipos. Por favor, recargue la página o informa a soporte.");
                return new List<Equipo>();
            }
        }

        public static List<Equipo> GetEquiposEnsayo(int idEquipoEnsayo)
        {
            //String consulta = @"SELECT id_equipo Id, nombre_equipo Nombre, idtipo_equipo IdTipo, equipo1.predefinido_equipoprocedimiento Predefinido
            //                        FROM equipos
            //                        INNER JOIN equipo_procedimiento as equipo1 ON id_equipo = equipo1.idequipo_equipoprocedimiento
            //                        INNER JOIN equipo_procedimiento as equipo2 ON equipo1.idprocedimiento_equipoprocedimiento= equipo2.idprocedimiento_equipoprocedimiento
            //                        WHERE equipoensayo_equipo=false and equipo2.idequipo_equipoprocedimiento=:IdEquipo";

            String consulta = @"SELECT id_equipo Id, nombre_equipo Nombre, idtipo_equipo IdTipo, equipo1.predefinido_equipoprocedimiento Predefinido
                                    FROM equipos
                                    INNER JOIN equipo_procedimiento as equipo1 ON id_equipo = equipo1.idequipo_equipoprocedimiento
                                    INNER JOIN equipo_procedimiento as equipo2 ON equipo1.idprocedimiento_equipoprocedimiento= equipo2.idprocedimiento_equipoprocedimiento
                                    WHERE id_equipo<>:IdEquipo and equipo2.idequipo_equipoprocedimiento=:IdEquipo";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<Equipo>(consulta, new { IdEquipo = idEquipoEnsayo }).ToList();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los equipos. Por favor, recargue la página o informa a soporte.");
                return new List<Equipo>();
            }
        }
    }

    [TableProperties("equipo_procedimiento")]
    public class EquipoProcedimiento : PersistenceData
    {
        [ColumnProperties("id_equipoprocedimiento", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("idprocedimiento_equipoprocedimiento")]
        public int IdProcedimiento { get; set; }

        [ColumnProperties("idequipo_equipoprocedimiento")]
        public int IdEquipo { get; set; }

        [ColumnProperties("predefinido_equipoprocedimiento")]
        public Boolean? Predefinido { get; set; }
    }
}
