using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> Attribute for table properties. </summary>
    /// <remarks> Oscvic, 11/02/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class)]
    public class TablePropertiesAttribute : System.Attribute
    {
        /// <summary> The name. </summary>
        public readonly string Name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------
        public TablePropertiesAttribute(string name)
        {
            this.Name = name;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> Attribute for column properties. </summary>
    /// <remarks> Oscvic, 11/02/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnPropertiesAttribute : System.Attribute
    {
        /// <summary> The name. </summary>
        public readonly string Name;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this ColumnPropertiesAttribute is identifier. </summary>
        /// <value> true if this ColumnPropertiesAttribute is identifier, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public Boolean IsId { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this ColumnPropertiesAttribute is
        ///           autonumeric. </summary>
        /// <value> true if this ColumnPropertiesAttribute is autonumeric, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public Boolean IsAutonumeric { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the format to use. </summary>
        /// <value> The format. </value>
        ///-------------------------------------------------------------------------------------------------
        public String Format { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="name"> The name. </param>
        ///-------------------------------------------------------------------------------------------------
        public ColumnPropertiesAttribute(string name)
        {
            this.Name = name;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary> A persistent data utility. </summary>
    /// <remarks> Oscvic, 11/02/2016. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public static class PersistentAttributesUtil
    {
        #region Static Métods for attributes

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets table name. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <typeparam name="Type"> Type you want to get data. </typeparam>
        /// <returns> The table name. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TablePropertiesInfo GetTableName<Type>() where Type : PersistenceData => PersistentAttributesUtil.GetTableName(typeof(Type));

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets table name. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="type"> Type you want to get data. </param>
        /// <returns> The table name. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TablePropertiesInfo GetTableName(Type type)
        {
            TablePropertiesAttribute att = type.GetCustomAttribute<TablePropertiesAttribute>();

            if (att != null)
                return new TablePropertiesInfo(att.Name, type.Name);

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets table columns. If withId is true, it gives the Id columns too. (default true) </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <typeparam name="Type"> Type you want to get data. </typeparam>
        /// <param name="withId"> true to with identifier. </param>
        /// <returns> An array of string. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ColumnPropertiesInfo[] GetTableColumns<Type>(Boolean withId = true) where Type : PersistenceData => PersistentAttributesUtil.GetTableColumns(typeof(Type), withId);

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets table columns. If withId is true, it gives the Id columns too. (default true) </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="type">   Type you want to get data. </param>
        /// <param name="withId"> true to with identifier. </param>
        /// <returns> An array of string. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ColumnPropertiesInfo[] GetTableColumns(Type type, Boolean withId = true)
        {
            return type.GetProperties()
                .Where(p => p.GetCustomAttribute<ColumnPropertiesAttribute>() != null)
                .Select(p => new { Name = p.Name, Att = p.GetCustomAttribute<ColumnPropertiesAttribute>() })
                .Where(obj => (!withId && !obj.Att.IsId) || withId)
                .Select(obj => new ColumnPropertiesInfo(obj.Name, obj.Att))
                .ToArray();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets table columns. If withId is true, it gives the Id columns too. (default true) </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="type">   Type you want to get data. </param>
        /// <param name="withId"> true to with identifier. </param>
        /// <returns> An array of string. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ColumnPropertiesInfo GetTableColumn(Type type, String propertyName)
        {

            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            ColumnPropertiesAttribute att = property.GetCustomAttribute<ColumnPropertiesAttribute>();

            if (att != null)
                return new ColumnPropertiesInfo(property.Name, att);

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets identifier column. The first it founds. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <typeparam name="Type"> Type of the type. </typeparam>
        /// <returns> The identifier column. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ColumnPropertiesInfo GetIdColumn<Type>() where Type : PersistenceData => PersistentAttributesUtil.GetIdColumn(typeof(Type));

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets identifier column. The first it founds. </summary>
        /// <remarks> Oscvic, 11/02/2016. </remarks>
        /// <param name="type"> The type. </param>
        /// <returns> The identifier column. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ColumnPropertiesInfo GetIdColumn(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new { Name = p.Name, Att = p.GetCustomAttribute<ColumnPropertiesAttribute>() })
                .Where(obj => obj.Att != null && obj.Att.IsId)
                .Select(obj => new ColumnPropertiesInfo(obj.Name, obj.Att)).FirstOrDefault();
        }

        #endregion
    }
}
