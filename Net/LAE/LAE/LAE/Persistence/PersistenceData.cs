using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Cartif.Extensions;
using Cartif.Util;
using Cartif.EasyDatabase;
using Cartif.Logs;
using System.Globalization;
using Dapper;
using DapperExtensions;
using Npgsql;
using Persistence;
using LAE.Modelo;
using System.Runtime.Serialization;

namespace Persistence
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A dato lae. </summary>
    /// <remarks> Oscvic, 2016-01-29. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class PersistenceData : IFormattable
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary> Formats the value of the current instance using the specified format. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="format"> The format to use.-or- A null reference (Nothing in Visual Basic)
        ///                       to use the default format defined for the type of the
        ///                       <see cref="T:System.IFormattable" /> implementation. </param>
        /// <returns> The value of the current instance in the specified format. </returns>
        ///-------------------------------------------------------------------------------------------------
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Formats the value of the current instance using the specified format. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <param name="format">         The format to use.-or- A null reference (Nothing in Visual Basic)
        ///                               to use the default format defined for the type of the
        ///                               <see cref="T:System.IFormattable" /> implementation. </param>
        /// <param name="formatProvider"> The provider to use to format the value.-or- A null reference
        ///                               (Nothing in Visual Basic) to obtain the numeric format
        ///                               information from the current locale setting of the operating
        ///                               system. </param>
        /// <returns> The value of the current instance in the specified format. </returns>
        ///-------------------------------------------------------------------------------------------------
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format != null)
            {
                PropertyInfo property = this.GetType().GetProperty(format, BindingFlags.Public | BindingFlags.Instance);
                String value = property?.GetValue(this)?.ToString();
                return value != null ? value : "";
            }
            return ToString();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Actualiza el Dato haciendo uso de Reflexión y del Id. El dato necesita los campos
        ///           IdDato, NombreTabla y Columnas. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <param name="toUpdateId"> The columns. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Boolean Update(int? toUpdateId = null) => InnerUpdate(toUpdateId: toUpdateId);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Actualiza el Dato haciendo uso de Reflexión y del Id. El dato necesita los campos
        ///           IdDato, NombreTabla y Columnas. No actualiza el Id. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <param name="columnsToUpdate"> The columns to update. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Boolean Update(params String[] columnsToUpdate) => InnerUpdate(columnsToUpdate);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Inner update. </summary>
        /// <remarks> Oscvic, 2016-02-01. </remarks>
        /// <param name="columnsToUpdate"> The columns to update. </param>
        /// <param name="toUpdateId">      The id of the row to update. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        private Boolean InnerUpdate(String[] columnsToUpdate = null, int? toUpdateId = null)
        {
            StringBuilder update = new StringBuilder("UPDATE ");
            try
            {
                Type type = this.GetType();

                /* El nombre del id del dato */
                ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(type);
                idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(this.GetType(), toUpdateId != null);
                columns.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

                if (columnsToUpdate != null && columnsToUpdate.Length > 0)
                    columns = columns.Where(c => columnsToUpdate.Contains(c.PropertyName)).ToArray();

                update.Append(tabla.DbName).Append(" SET ");

                foreach (var column in columns)
                    update.Append(column.DbName).Append("=@").Append(column.PropertyName).Append(",");

                /* Si toUpdateId (el id que se va a actualizar) no es null lo usamos para actualizar el registro en el where,
                 * si no el valor de la propiedad Id */
                String idValue = toUpdateId != null ? toUpdateId.ToString() : type.GetValueOfPropertyOf(idColumn.PropertyName, this)?.ToString();

                update.Replace(",", " WHERE ", update.Length - 1, 1).Append(idColumn.DbName).Append("=").Append(idValue);

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Execute(update.ToString(), this) == 1;
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query es: " + update, ex);
                return false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Borra el Dato usando el Id. El dato necesita los campos IdDato, NombreTabla y
        ///           Columnas. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Boolean Delete()
        {
            String delete = "";
            try
            {
                Type type = this.GetType();

                /* El nombre del id del dato */
                ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(type);
                idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

                /* El id del dato */
                String idValue = type.GetValueOfPropertyOf(idColumn.PropertyName, this)?.ToString();
                idValue.ThrowIfArgumentIsNull("El objeto debe tener un id válido para actualizarse");

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                delete = "DELETE FROM " + tabla.DbName + " WHERE " + idColumn.DbName + "=@" + idColumn.PropertyName;

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Execute(delete, this) == 1;
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query es: " + delete, ex);
                return false;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Inserta todos los valores del dato. Si isAutonumeric no se inserta el Id para que se
        ///           genere solo. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <param name="isAutonumeric"> true if this PersistenceData is autonumeric. </param>
        /// <returns> La nueva clave o -1 si no ha sido exitoso. </returns>
        ///-------------------------------------------------------------------------------------------------
        public int Insert(Boolean isAutonumeric = true) => InnerInsert(useIdColumn: !isAutonumeric);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Inserta el Dato con los datos indicados. Si entre las columnas no está el Id, este se
        ///           inserta autonuméricamente si es Autonumerico. El dato necesita los campos IdDato, NombreTabla y
        ///           Columnas. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <param name="columnsToInsert"> The columns to insert. </param>
        /// <returns> La nueva clave o -1 si no ha sido exitoso.. </returns>
        ///-------------------------------------------------------------------------------------------------
        public int Insert(params String[] columnsToInsert) => InnerInsert(columnsToInsert, true);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Inner insert. </summary>
        /// <remarks> Oscvic, 2016-01-29. </remarks>
        /// <param name="columnsToInsert"> The columns to insert. </param>
        /// <param name="useIdColumn">     The columns. </param>
        /// <returns> An int. </returns>
        ///-------------------------------------------------------------------------------------------------
        private int InnerInsert(String[] columnsToInsert = null, Boolean useIdColumn = false)
        {
            /* Crea un String Builder para la primera parte de la insert y otro para los values */
            StringBuilder insert = new StringBuilder("INSERT INTO ");
            StringBuilder values = new StringBuilder(" VALUES(");
            try
            {
                Type type = this.GetType();

                /* El nombre del id del dato */
                ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(type);
                idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

                /* El id del dato */
                String idValue = type.GetValueOfPropertyOf(idColumn.PropertyName, this)?.ToString();
                idValue.ThrowIfArgumentIsNull("El objeto debe tener un id válido para actualizarse");

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(this.GetType(), useIdColumn);
                columns.ThrowIfArgumentIsNull("El tipo debe poseer Attributos ColumnProperties");

                if (columnsToInsert != null && columnsToInsert.Length > 0)
                    columns = columns.Where(c => columnsToInsert.Contains(c.PropertyName)).ToArray();

                insert.Append(tabla.DbName).Append("(");

                Object value = null;
                Type valueType = null;
                /* Loop evitando el id */
                foreach (var column in columns)
                {
                    value = type.GetValueOfPropertyOf(column.PropertyName, this);
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
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    toReturn = conn.Query<int>(insert.ToString(), this).FirstOrDefault();

                return toReturn;
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "", ex);
                return -1;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Update all columns of the object. </summary>
        /// <remarks> 2016-03-10. </remarks>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal
        ///                                      values. </exception>
        /// <returns></returns>
        ///-------------------------------------------------------------------------------------------------
        public void Load()
        {
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(this.GetType());
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            Type type = this.GetType();
            PropertyInfo prop = type.GetProperty(idColumn.PropertyName);


            this.InnerLoad(idColumn, prop.GetValue(this));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Update columns of the object. </summary>
        /// <remarks> 2016-03-10. </remarks>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal
        ///                                      values. </exception>
        /// <param name="columnsToSelect"> The columns to select. </param>
        /// <returns></returns>
        ///-------------------------------------------------------------------------------------------------
        public void Load(params String[] columnsToSelect)
        {
            ColumnPropertiesInfo idColumn = PersistentAttributesUtil.GetIdColumn(this.GetType());
            idColumn.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo ColumnProperties con IsId = true");

            Type type = this.GetType();
            PropertyInfo prop = type.GetProperty(idColumn.PropertyName);

            this.InnerLoad(idColumn, prop.GetValue(this), columnsToSelect);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Update object. </summary>
        /// <remarks> 2016-03-10. </remarks>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal
        ///                                      values. </exception>
        /// <param name="propiedad">       The propiedad. </param>
        /// <param name="value">           The identifier. </param>
        /// <param name="columnsToSelect"> The columns to select. </param>
        /// <returns></returns>
        ///-------------------------------------------------------------------------------------------------
        private void InnerLoad(ColumnPropertiesInfo propiedad, Object value, String[] columnsToSelect = null)
        {
            StringBuilder select = new StringBuilder("SELECT ");

            if (propiedad != null)
                value.ThrowIfArgumentIsNull("Value no puede ser null");

            try
            {
                Type type = this.GetType();

                /* El id del dato */
                TablePropertiesInfo tabla = PersistentAttributesUtil.GetTableName(type);
                tabla.ThrowIfArgumentIsNull("El tipo debe poseer un Attributo TableProperties");

                /* Las Columnas de la tabla del dato */
                ColumnPropertiesInfo[] columns = PersistentAttributesUtil.GetTableColumns(type, true);
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

                dynamic recovered;

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    recovered = conn.Query<dynamic>(select.ToString(), parameters).FirstOrDefault();

                
                var dicRecovered = ((IDictionary<string, object>)recovered);

                foreach (var column in columns)
                {
                    PropertyInfo prop = type.GetProperty(column.PropertyName);
                    prop.SetValue(this, dicRecovered[column.PropertyName.ToLower()], null);
                }

            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + select, ex);
            }
        }

    }
}
