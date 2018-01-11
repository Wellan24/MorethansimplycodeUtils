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

        public Boolean Delete(NpgsqlConnection connection = null) => PersistenceManager.Delete(this, connection);

        public int Insert(NpgsqlConnection connection = null, Boolean isAutonumeric = true) => PersistenceManager.Insert(this, connection, isAutonumeric);

        public int Insert(NpgsqlConnection connection = null, params String[] columnsToInsert) => PersistenceManager.Insert(this, connection, columnsToInsert);

        public void Load(NpgsqlConnection connection = null, params String[] columnsToLoad) => PersistenceManager.Load(this, connection, columnsToLoad);

        public Boolean Update(NpgsqlConnection connection = null, int? toUpdateId = null) => PersistenceManager.Update(this, connection, toUpdateId: toUpdateId);

        public Boolean Update(NpgsqlConnection connection = null, params String[] columnsToUpdate) => PersistenceManager.Update(this, connection, columnsToUpdate);

    }
}
