using Cartif.Collections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A tipo log. </summary>
    /// <remarks> Oscvic, 2016-01-11. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class TipoLog : ConfigurationElement
    {
        private static CartifPropertyList<String, TipoLog> tipos;   /* The tipos */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Static constructor. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        ///--------------------------------------------------------------------------------------------------
        static TipoLog() { tipos = new CartifPropertyList<String, TipoLog>(tipo => tipo.Nombre); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds a tipo log. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        /// <param name="tipo"> The tipo. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void RegisterTipoLog(TipoLog tipo) { tipos.Add(tipo); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds a tipo log. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        /// <param name="tipoLogs"> The tipo logs. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void RegisterTiposLog(TipoLog[] tipoLogs) { tipos.AddRange(tipoLogs); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Froms. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        /// <param name="nombre"> La carpeta donde se guardarán los logs. </param>
        /// <returns> A TipoLog. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TipoLog From(String nombre) { return tipos[nombre]; }

        /// <summary>
        /// La carpeta donde se guardarán los logs
        /// </summary>
        public String Nombre;   /* The nombre */

        public String RutaRaiz; /* The ruta raiz */

        public String FileFormat;   /* The file format */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="nombre">     La carpeta donde se guardarán los logs. </param>
        /// <param name="rutaRaiz">   The ruta raiz. </param>
        /// <param name="fileFormat"> The file format. </param>
        ///--------------------------------------------------------------------------------------------------
        public TipoLog(String nombre, String rutaRaiz, String fileFormat)
        {
            Nombre = nombre;
            RutaRaiz = rutaRaiz;
            FileFormat = fileFormat;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Equality operator. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="f1"> The first EnumerationFileLine. </param>
        /// <param name="f2"> The second EnumerationFileLine. </param>
        /// <returns> The result of the operation. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean operator ==(TipoLog f1, TipoLog f2) { return TipoLog.Equals(f1, f2); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Inequality operator. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="f1"> The first EnumerationFileLine. </param>
        /// <param name="f2"> The second EnumerationFileLine. </param>
        /// <returns> The result of the operation. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean operator !=(TipoLog f1, TipoLog f2) { return !TipoLog.Equals(f1, f2); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Indica si esta instancia y un objeto especificado son iguales. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="obj"> Otro objeto con el que se va a realizar la comparación. </param>
        /// <returns> true si <paramref name="obj" /> y esta instancia son del mismo tipo y representan el mismo
        ///           valor; en caso contrario, false. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            return TipoLog.Equals(this, obj);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Devuelve el código hash de esta instancia. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <returns> Entero de 32 bits con signo, que es el código hash de esta instancia. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override int GetHashCode() { return this.Nombre.GetHashCode(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Indica si esta instancia y un objeto especificado son iguales. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        /// <param name="f">   Tipo log to be compared. </param>
        /// <param name="obj"> Otro objeto con el que se va a realizar la comparación. </param>
        /// <returns> true si <paramref name="obj" /> y esta instancia son del mismo tipo y representan el mismo
        ///           valor; en caso contrario, false. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean Equals(TipoLog f, Object obj)
        {
            if (Object.ReferenceEquals(f, obj))
                return true;

            if (Object.ReferenceEquals(f, null) || obj == null)
                return false;

            if (obj.GetType().Equals(typeof(TipoLog)))
            {
                TipoLog other = (TipoLog)obj;

                if (other.Nombre != null && other.RutaRaiz != null)
                    return other.Nombre.Equals(f.Nombre) && other.RutaRaiz.Equals(f.RutaRaiz);
            }

            return false;
        }
    }
}
