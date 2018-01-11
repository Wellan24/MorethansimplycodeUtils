using Cartif.EasyDatabase;
using Cartif.Logs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LAE.Comun.Persistence
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A base datos lae. </summary>
    /// <remarks> Oscvic, 2016-02-01. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class PersistenceDataBase
    {
        /// <summary>
        /// El string de conexión a la base de datos. En caso de que se pase a usar un web service, deberá pasar al web.config.
        /// Como el app.config está disponible para que lo habrán los usuarios, lo incrustamos en código, que es algo más difícil de obtener.
        /// </summary>
        public static readonly string STRING_CONNECTION =
            (ConfigurationManager.AppSettings["LOCATION"] != "RELEASE")
            ? "Server=192.168.109.109;Port=5432;Database=LAE;UserId=postgres;Password=bBdD_pg91%SqL;Pooling=true;MinPoolSize=1;MaxPoolSize=5;"
            : "Server=thot;Port=5432;Database=LAE;UserId=gestion;Password=f0e13dca45;Pooling=true;MinPoolSize=1;MaxPoolSize=5;";    /* . */

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Devuelve un objeto NpgsqlConnection ya abierto o null si no se ha podido conectar. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <returns> NpgsqlConnection o null si no se ha podido conectar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static NpgsqlConnection GetConnection()
        {

            NpgsqlConnection conexion = null;
            try
            {
                conexion = new NpgsqlConnection(STRING_CONNECTION);
                conexion.Open();
            }
            catch (Exception ex)
            {

                if (conexion != null && conexion.State.HasFlag(ConnectionState.Open))
                    conexion.Close();
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error conectando", ex);
            }

            return conexion;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets easy connection. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <returns> The easy connection. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static EasyConnection GetEasyConnection()
        {
            EasyConnection conexion = null;
            try
            {
                conexion = new EasyConnection(new NpgsqlConnection(STRING_CONNECTION));
                conexion.Open();
            }
            catch (Exception ex)
            {
                if (conexion != null && conexion.IsOpen)
                    conexion.Dispose();
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error conectando", ex);
            }

            return conexion;
        }

    }
}
