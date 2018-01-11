using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Cartif.Extensions;


namespace Cartif.EasyDatabase
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An easy connection. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class EasyConnection : IDisposable
    {
        private DbConnection innerDbConnection; /* The inner database connection */

        public Boolean IsOpen { get { return innerDbConnection.State.HasFlag(ConnectionState.Open); } }
        public ConnectionState State { get { return innerDbConnection.State; } }
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="connection"> The connection. </param>
        ///--------------------------------------------------------------------------------------------------
        public EasyConnection(DbConnection connection)
        {
            innerDbConnection = connection;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Finaliser. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        ~EasyConnection()
        {
            innerDbConnection.Dispose();
            innerDbConnection.Close();

            innerDbConnection = null;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the open. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> An EasyConnection. </returns>
        ///--------------------------------------------------------------------------------------------------
        public EasyConnection Open()
        {
            if (!innerDbConnection.State.HasFlag(ConnectionState.Open))
                innerDbConnection.Open();
            return this;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the easy query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="actionPerRecord"> The action per record. </param>
        /// <param name="query">           The query. </param>
        ///--------------------------------------------------------------------------------------------------
        public void ExecuteEasyQuery(Action<DbDataReader> actionPerRecord, String query)
        {
            DbDataReader rdr = null;
            DbCommand cmd = null;

            try
            {
                cmd = innerDbConnection.CreateCommand();
                cmd.CommandText = query;

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                    actionPerRecord(rdr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the easy query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="actionPerRecord"> The action per record. </param>
        /// <param name="query">           The query. </param>
        /// <param name="args">            A variable-length parameters list containing arguments. </param>
        ///--------------------------------------------------------------------------------------------------
        public void ExecuteEasyQuery(Action<DbDataReader> actionPerRecord, String query, params Object[] args)
        {
            query = String.Format(query, (Object[])args);
            ExecuteEasyQuery(actionPerRecord, query);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the easy non query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="query"> The query. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean ExecuteEasyNonQuery(String query)
        {
            DbCommand cmd = null;
            DbTransaction ot = null;

            try
            {
                ot = innerDbConnection.BeginTransaction();

                cmd = innerDbConnection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 30;

                Boolean ok = cmd.ExecuteNonQuery() > 0;
                ot.Commit();

                return ok;
            }
            catch (Exception)
            {
                ot.Rollback();
                throw;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the easy non query operation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="query"> The query. </param>
        /// <param name="args">  A variable-length parameters list containing arguments. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean ExecuteEasyNonQuery(String query, params Object[] args)
        {
            query = String.Format(query, (Object[])args);
            return ExecuteEasyNonQuery(query);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Realiza tareas definidas por la aplicación asociadas a la liberación o al restablecimiento
        ///           de recursos no administrados. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            innerDbConnection.Dispose();
            innerDbConnection.Close();

            innerDbConnection = null;

            GC.SuppressFinalize(this);
        }
    }
}
