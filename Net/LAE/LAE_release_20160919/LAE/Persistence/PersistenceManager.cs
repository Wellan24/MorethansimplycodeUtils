using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;
using Cartif.EasyDatabase;
using Cartif.Util;
using Cartif.Logs;
using Dapper;
using Npgsql;
using System.Reflection;

namespace Persistence
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A manager. </summary>
    /// <remarks> Oscvic, 2016-02-01. </remarks>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public class PersistenceManager
    {
        #region Select

        public static T SelectByID<T>(Object id, NpgsqlConnection connection = null) where T : PersistenceData
        {
            /* El nombre del id del dato */
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(typeof(T));
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            return SelectByProperty<T>(idColumn, id, connection).FirstOrDefault();
        }

        public static IEnumerable<T> SelectByProperty<T>(ColumnPropertiesInfo propiedad, Object value, NpgsqlConnection connection = null) where T : PersistenceData
        {
            return InnerSelect<T>(propiedad, value, connection: connection);
        }

        public static T SelectByID<T>(Object id, NpgsqlConnection connection = null, params String[] columnsToSelect) where T : PersistenceData
        {
            /* El nombre del id del dato */
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(typeof(T));
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            return SelectByProperty<T>(idColumn.PropertyName, id, connection, columnsToSelect).FirstOrDefault();
        }

        public static IEnumerable<T> SelectByProperty<T>(String propiedad, Object value, NpgsqlConnection connection = null, params String[] columnsToSelect) where T : PersistenceData
        {
            /* Las Columnas de la tabla del dato */
            ColumnPropertiesInfo column = PersistentAttributesUtil.GetTableColumn(typeof(T), propiedad);
            column.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

            return InnerSelect<T>(column, value, columnsToSelect, connection: connection);
        }

        public static IEnumerable<T> SelectAll<T>(NpgsqlConnection connection = null, params String[] columnsToSelect) where T : PersistenceData
        {
            return InnerSelect<T>(null, null, columnsToSelect: columnsToSelect, connection: connection);
        }

        private static IEnumerable<T> InnerSelect<T>(ColumnPropertiesInfo propiedad, Object value, String[] columnsToSelect = null, NpgsqlConnection connection = null) where T : PersistenceData
        {
            StringBuilder select = new StringBuilder("SELECT ");

            if (propiedad != null)
                value.ThrowIfArgumentIsNull("Value no puede ser null");

            try
            {
                Type type = typeof(T);

                /* SELECT */
                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(typeof(T), true);
                columns.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

                if (columnsToSelect != null && columnsToSelect.Length > 0)
                    columns = columns.Where(c => columnsToSelect.Contains(c.PropertyName)).ToArray();

                foreach (var column in columns)
                    select.Append(column.DbName).Append(" ").Append(column.PropertyName).Append(",");

                select.Replace(",", " FROM ", select.Length - 1, 1).Append(tabla.DbName);

                /* WHERE */
                OneParameter parameters = new OneParameter();

                if (propiedad != null)
                {
                    select.Append(" WHERE ");

                    if (value is String)
                        select.Append(propiedad.DbName).Append(" LIKE @Value");
                    else
                        select.Append(propiedad.DbName).Append("=@Value");

                    parameters = new OneParameter() { Value = value };
                }

                /* QUERY */
                if (connection != null)
                    return connection.Query<T>(select.ToString(), parameters);

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<T>(select.ToString(), parameters);
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + select, ex);
                throw;
            }
        }

        #endregion

        #region Delete

        public static Boolean Delete(PersistenceData data, NpgsqlConnection connection = null)
        {
            String delete = "";
            try
            {
                data.ThrowIfArgumentIsNull("Debes proporcionar un objeto que borrar!");
                Type type = data.GetType();

                /* El nombre del id del dato */
                ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(type);
                idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

                /* El id del dato */
                String idValue = type.GetValueOfPropertyOf(idColumn.PropertyName, data)?.ToString();
                idValue.ThrowIfArgumentIsNull("El objeto debe tener un id válido para actualizarse");

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                delete = "DELETE FROM " + tabla.DbName + " WHERE " + idColumn.DbName + "=@" + idColumn.PropertyName;

                if (connection != null)
                    return connection.Execute(delete, data) == 1;

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Execute(delete, data) == 1;
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query es: " + delete, ex);
                throw;
            }
        }

        #endregion

        #region Insert

        public static int Insert(PersistenceData data, NpgsqlConnection connection = null, Boolean isAutonumeric = true) => InnerInsert(data, useIdColumn: !isAutonumeric, connection: connection);

        public static int Insert(PersistenceData data, NpgsqlConnection connection = null, params String[] columnsToInsert) => InnerInsert(data, columnsToInsert, true, connection);

        private static int InnerInsert(PersistenceData data, String[] columnsToInsert = null, Boolean useIdColumn = false, NpgsqlConnection connection = null)
        {
            /* Crea un String Builder para la primera parte de la insert y otro para los values */
            StringBuilder insert = new StringBuilder("INSERT INTO ");
            StringBuilder values = new StringBuilder(" VALUES(");
            try
            {
                data.ThrowIfArgumentIsNull("Debes proporcionar un objeto que borrar!");
                Type type = data.GetType();

                /* El nombre del id del dato */
                ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(type);
                idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

                /* El id del dato */
                String idValue = type.GetValueOfPropertyOf(idColumn.PropertyName, data)?.ToString();
                idValue.ThrowIfArgumentIsNull("El objeto debe tener un id válido para actualizarse");

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(type, useIdColumn);
                columns.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

                if (columnsToInsert != null && columnsToInsert.Length > 0)
                    columns = columns.Where(c => columnsToInsert.Contains(c.PropertyName)).ToArray();

                insert.Append(tabla.DbName).Append("(");

                Object value = null;
                Type valueType = null;
                /* Loop evitando el id */
                foreach (var column in columns)
                {
                    value = type.GetValueOfPropertyOf(column.PropertyName, data);
                    valueType = value?.GetType();

                    /* Solo añadimos si encontramos un valor */
                    if (valueType != null)
                    {
                        /* Añadimos la columna a la primera parte */
                        insert.Append(column.DbName).Append(",");

                        /* Añadimos el valor a la segunda parte */
                        values.Append("@" + column.PropertyName).Append(",");
                    }
                }

                /* Cerramos los parentesis */
                insert.Replace(",", ")", insert.Length - 1, 1);
                insert.Append(values.Replace(",", ")", values.Length - 1, 1));

                /* Añadimos un returning para obtener el id que se ha generado */
                insert.Append(" returning ").Append(idColumn.DbName);
                int toReturn = -1;

                if (connection != null)
                {
                    toReturn = connection.Query<int>(insert.ToString(), data).FirstOrDefault();
                }
                else {
                    using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                        toReturn = conn.Query<int>(insert.ToString(), data).FirstOrDefault();
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "", ex);
                throw;
            }
        }

        #endregion

        #region Load

        public static void Load(PersistenceData data, NpgsqlConnection connection = null, params String[] columnsToLoad)
        {
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(data.GetType());
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            Type type = data.GetType();
            PropertyInfo prop = type.GetProperty(idColumn.PropertyName);

            InnerLoad(data, idColumn, prop.GetValue(data), connection, columnsToLoad);
        }

        private static void InnerLoad(PersistenceData data, ColumnPropertiesInfo propiedad, Object value, NpgsqlConnection connection = null, String[] columnsToLoad = null)
        {
            StringBuilder select = new StringBuilder("SELECT ");

            if (propiedad != null)
                value.ThrowIfArgumentIsNull("Value no puede ser null");

            try
            {
                data.ThrowIfArgumentIsNull("Debes proporcionar un objeto que borrar!");
                Type type = data.GetType();

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(type, true);
                columns.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

                if (columnsToLoad != null && columnsToLoad.Length > 0)
                    columns = columns.Where(c => columnsToLoad.Contains(c.PropertyName)).ToArray();

                foreach (var column in columns)
                    select.Append(column.DbName).Append(" ").Append(column.PropertyName).Append(",");

                select.Replace(",", " FROM ", select.Length - 1, 1).Append(tabla.DbName);

                OneParameter parameters = new OneParameter();

                if (propiedad != null)
                {
                    select.Append(" WHERE ");

                    if (value is String)
                        select.Append(propiedad.DbName).Append(" LIKE @Value");
                    else
                        select.Append(propiedad.DbName).Append("=@Value");

                    parameters = new OneParameter() { Value = value };
                }

                dynamic recovered;

                if (connection != null)
                {
                    recovered = connection.Query<dynamic>(select.ToString(), parameters).FirstOrDefault();
                }
                else
                {
                    using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                        recovered = conn.Query<dynamic>(select.ToString(), parameters).FirstOrDefault();
                }

                var dicRecovered = ((IDictionary<string, object>)recovered);

                foreach (var column in columns)
                {
                    PropertyInfo prop = type.GetProperty(column.PropertyName);
                    prop.SetValue(data, dicRecovered[column.PropertyName.ToLower()], null);
                }

            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + select, ex);
                throw;
            }
        }

        #endregion

        #region Update

        public static Boolean Update(PersistenceData data, NpgsqlConnection connection = null, int? toUpdateId = null) =>
            InnerUpdate(data, connection, toUpdateId: toUpdateId);

        public static Boolean Update(PersistenceData data, NpgsqlConnection connection = null, params String[] columnsToUpdate) =>
            InnerUpdate(data, connection, columnsToUpdate);

        private static Boolean InnerUpdate(PersistenceData data, NpgsqlConnection connection = null, String[] columnsToUpdate = null, int? toUpdateId = null)
        {
            StringBuilder update = new StringBuilder("UPDATE ");
            try
            {
                Type type = data.GetType();

                /* El nombre del id del dato */
                ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(type);
                idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(type, toUpdateId != null);
                columns.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

                if (columnsToUpdate != null && columnsToUpdate.Length > 0)
                    columns = columns.Where(c => columnsToUpdate.Contains(c.PropertyName)).ToArray();

                update.Append(tabla.DbName).Append(" SET ");

                foreach (var column in columns)
                    update.Append(column.DbName).Append("=@").Append(column.PropertyName).Append(",");

                /* Si toUpdateId (el id que se va a actualizar) no es null lo usamos para actualizar el registro en el where,
                 * si no el valor de la propiedad Id */
                String idValue = toUpdateId != null ? toUpdateId.ToString() : type.GetValueOfPropertyOf(idColumn.PropertyName, data)?.ToString();

                update.Replace(",", " WHERE ", update.Length - 1, 1).Append(idColumn.DbName).Append("=").Append(idValue);

                if (connection != null)
                    return connection.Execute(update.ToString(), data) == 1;

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Execute(update.ToString(), data) == 1;
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query es: " + update, ex);
                throw;
            }
        }

        #endregion
    }

    class OneParameter { public Object Value { get; set; } }
}
