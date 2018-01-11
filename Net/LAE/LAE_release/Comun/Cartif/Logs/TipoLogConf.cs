using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Collection of tipo log confs. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class TipoLogConfCollection : ConfigurationElementCollection
    {
        const string ELEMENT_NAME = "TipoLog";  /* Name of the element */

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
            return new TipoLogConf();
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
            return ((TipoLogConf)element).Nombre;
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A tipo log conf. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class TipoLogConf : ConfigurationElement
    {
        const string NOMBRE = "Nombre"; /* The nombre */
        const string RUTA_RAIZ = "RutaRaiz";    /* The ruta raiz */
        const string FILE_FORMAT = "FileFormat";    /* The file format */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the nombre. </summary>
        /// <value> The nombre. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(NOMBRE, IsRequired = true)]
        public string Nombre
        {
            get { return (String)base[NOMBRE]; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the ruta raiz. </summary>
        /// <value> The ruta raiz. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(RUTA_RAIZ)]
        public String RutaRaiz
        {
            get { return (String)base[RUTA_RAIZ]; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the file format. </summary>
        /// <value> The file format. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(FILE_FORMAT)]
        public String FileFormat
        {
            get { return (String)base[FILE_FORMAT]; }
        }
    }
}
