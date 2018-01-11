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

namespace Persistence
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A manager. </summary>
    /// <remarks> Oscvic, 2016-02-01. </remarks>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public class PersistenceManager<T> where T : PersistenceData
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Select by identifier. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <param name="id"> The identifier. </param>
        /// <returns> A T. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T SelectByID(Object id)
        {
            /* El nombre del id del dato */
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(typeof(T));
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            return SelectByProperty(idColumn, id).FirstOrDefault();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Select by property. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <param name="propiedad"> The propiedad. </param>
        /// <param name="value">     The identifier. </param>
        /// <returns> A T. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IEnumerable<T> SelectByProperty(ColumnPropertiesInfo propiedad, Object value)
        {
            return InnerSelect(propiedad, value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Select by identifier. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <param name="id">              The identifier. </param>
        /// <param name="columnsToSelect"> The columns to select. </param>
        /// <returns> A T. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T SelectByID(Object id, params String[] columnsToSelect)
        {
            /* El nombre del id del dato */
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(typeof(T));
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            return SelectByProperty(idColumn.PropertyName, id, columnsToSelect).FirstOrDefault();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Select by property. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <param name="propiedad">       The propiedad. </param>
        /// <param name="value">           The identifier. </param>
        /// <param name="columnsToSelect"> The columns to select. </param>
        /// <returns> A T. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IEnumerable<T> SelectByProperty(String propiedad, Object value, params String[] columnsToSelect)
        {
            /* Las Columnas de la tabla del dato */
            ColumnPropertiesInfo column = PersistentAttributesUtil.GetTableColumn(typeof(T), propiedad);
            column.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

            return InnerSelect(column, value, columnsToSelect);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Select all. </summary>
        /// <remarks> Oscvic, 2016-03-03. </remarks>
        /// <param name="columnsToSelect"> The columns to select. </param>
        /// <returns> A T. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IEnumerable<T> SelectAll(params String[] columnsToSelect)
        {
            /* Las Columnas de la tabla del dato */
            //ColumnPropertiesInfo column = PersistentAttributesUtil.GetTableColumn(typeof(T), null);
            //column.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

            return InnerSelect(null, null, columnsToSelect);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Enumerates inner select in this collection. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal
        ///                                      values. </exception>
        /// <param name="propiedad">       The propiedad. </param>
        /// <param name="value">           The identifier. </param>
        /// <param name="columns">         The columns. </param>
        /// <param name="columnsToSelect"> The columns to select. </param>
        /// <returns> An enumerator that allows foreach to be used to process inner select in this collection. </returns>
        ///-------------------------------------------------------------------------------------------------
        private static IEnumerable<T> InnerSelect(ColumnPropertiesInfo propiedad, Object value, String[] columnsToSelect = null)
        {
            StringBuilder select = new StringBuilder("SELECT ");

            if (propiedad != null)
                value.ThrowIfArgumentIsNull("Value no puede ser null");

            try
            {
                Type type = typeof(T);

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

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<T>(select.ToString(), parameters);
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + select, ex);
                return null;
            }
        }
    }

    class OneParameter { public Object Value { get; set; } }
}
