using Cartif.EasyDatabase;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A database connection extensions. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class DbConnectionExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DbConnection extension method that executes the easy query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="connection">      The connection to act on. </param>
        /// <param name="actionPerRecord"> The action per record. </param>
        /// <param name="query">           The query. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ExecuteEasyQuery(this DbConnection connection, Action<DbDataReader> actionPerRecord, String query)
        {
            new EasyConnection(connection).Open().ExecuteEasyQuery(actionPerRecord, query);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DbConnection extension method that executes the easy query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="connection">      The connection to act on. </param>
        /// <param name="actionPerRecord"> The action per record. </param>
        /// <param name="query">           The query. </param>
        /// <param name="args">            A variable-length parameters list containing arguments. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void ExecuteEasyQuery(this DbConnection connection, Action<DbDataReader> actionPerRecord, String query, params Object[] args)
        {
            new EasyConnection(connection).Open().ExecuteEasyQuery(actionPerRecord, query, (Object[])args);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DbConnection extension method that executes the easy non query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="connection"> The connection to act on. </param>
        /// <param name="query">      The query. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean ExecuteEasyNonQuery(this DbConnection connection, String query)
        {
            return new EasyConnection(connection).Open().ExecuteEasyNonQuery(query);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A DbConnection extension method that executes the easy non query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="connection"> The connection to act on. </param>
        /// <param name="query">      The query. </param>
        /// <param name="args">       A variable-length parameters list containing arguments. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean ExecuteEasyNonQuery(this DbConnection connection, String query, params String[] args)
        {
            return new EasyConnection(connection).Open().ExecuteEasyNonQuery(query, (Object[])args);
        }
    }
}
