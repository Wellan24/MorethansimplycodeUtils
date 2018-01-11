using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A log. </summary>
    /// <remarks> Oscvic, 2016-01-11. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class Log
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the location. </summary>
        /// <value> The location. </value>
        ///--------------------------------------------------------------------------------------------------
        public MethodBase Method { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the mensaje log. </summary>
        /// <value> The mensaje log. </value>
        ///--------------------------------------------------------------------------------------------------
        public String MensajeLog { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the excepcion. </summary>
        /// <value> The excepcion. </value>
        ///--------------------------------------------------------------------------------------------------
        public Exception Excepcion { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the Date/Time of the fecha. </summary>
        /// <value> The fecha. </value>
        ///--------------------------------------------------------------------------------------------------
        public DateTime Fecha { get; set; }

        private const String defaultFormat = "[ {0:dd/MM/yyyy HH:mm:ss} || {1} => {2} ]  Mensaje: {3}\n{4}";    /* The mensaje */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Crea un log con la fecha actual. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="method"> The location. </param>
        /// <param name="log">    El mensaje del log. </param>
        ///--------------------------------------------------------------------------------------------------
        public Log(MethodBase method, String log)
        {
            Method = method;
            MensajeLog = log;
            Fecha = DateTime.Now;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Crea un log con la fecha actual. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="method"> The location. </param>
        /// <param name="log">    El mensaje del log. </param>
        /// <param name="ex">     The ex. </param>
        ///--------------------------------------------------------------------------------------------------
        public Log(MethodBase method, String log, Exception ex)
            : this(method, log)
        {
            Excepcion = ex;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Log() { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Devuelve un string con el formato [yyyy-MM-dd] Location: {localizacion} || Log: {log} </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <returns> string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return ToString(defaultFormat);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Devuelve un string con el formato pasado como argumento. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="format"> Describes the format to use. </param>
        /// <returns> string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public string ToString(String format)
        {
            try
            {
                format = format.IsNotNullOrEmpty() ? format : defaultFormat;
                return String.Format(format, Fecha, Method.DeclaringType.FullName, Method.Name, MensajeLog, Excepcion != null ? "\n" + Excepcion : "");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
