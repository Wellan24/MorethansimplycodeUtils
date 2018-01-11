using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Collection of logger confs. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class LoggerConfCollection : ConfigurationElementCollection
    {
        const string ELEMENT_NAME = "Logger";   /* Name of the element */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el tipo de <see cref="T:System.Configuration.ConfigurationElementCollection" />. </summary>
        /// <value> Enumerador <see cref="T:System.Configuration.ConfigurationElementCollectionType" /> de
        ///         esta colección. </value>
        ///--------------------------------------------------------------------------------------------------
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el nombre que se utiliza para identificar esta colección de elementos en el
        ///           archivo de configuración cuando se reemplaza en una clase derivada. </summary>
        /// <value> Nombre de la colección; de lo contrario, una cadena vacía. El valor predeterminado es una
        ///         cadena vacía. </value>
        ///--------------------------------------------------------------------------------------------------
        protected override String ElementName
        {
            get { return ELEMENT_NAME; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Cuando se reemplaza en una clase derivada, se crea un nuevo objeto
        ///           <see cref="T:System.Configuration.ConfigurationElement" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> Un nuevo objeto <see cref="T:System.Configuration.ConfigurationElement" />. </returns>
        ///--------------------------------------------------------------------------------------------------
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerConf();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Cuando se reemplaza en una clase derivada, obtiene la clave de elemento para un elemento
        ///           de configuración especificado. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="element"> Objeto <see cref="T:System.Configuration.ConfigurationElement" /> para el
        ///                        que se va a devolver la clave. </param>
        /// <returns> <see cref="T:System.Object" /> que actúa como clave para el objeto
        ///           <see cref="T:System.Configuration.ConfigurationElement" /> especificado. </returns>
        ///--------------------------------------------------------------------------------------------------
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoggerConf)element).Type;
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A logger conf. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class LoggerConf : ConfigurationElement
    {
        const string ID = "Id"; /* The identifier */
        const string TYPE_NAME = "TypeName";    /* Name of the type */
        const string FORMAT = "Format"; /* Describes the format to use */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the identifier. </summary>
        /// <value> The identifier. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(ID, IsRequired = true)]
        public int Id
        {
            get { return int.Parse(this[ID].ToString()); }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the name of the type. </summary>
        /// <value> The name of the type. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(TYPE_NAME, IsRequired = true)]
        public String TypeName
        {
            get
            {

                return (String)this[TYPE_NAME];

            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the type. </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or illegal
        ///                                      values. </exception>
        /// <value> The type. </value>
        ///--------------------------------------------------------------------------------------------------
        public Type Type
        {
            get
            {
                try
                { return Type.GetType(TypeName); }
                catch (Exception ex)
                {
                    throw new ArgumentException("The Type is bad formatted", ex);
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the format to use. </summary>
        /// <value> The format. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(FORMAT, IsRequired = true)]
        public String Format
        {
            get { return (String)this[FORMAT]; }
        }
    }
}
