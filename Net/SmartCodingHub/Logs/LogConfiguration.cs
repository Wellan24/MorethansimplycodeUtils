using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A log configuration. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class LogConfiguration : ConfigurationSection
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Configures this LogConfiguration. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <exception cref="FormatException"> Thrown when the format of the ? is incorrect. </exception>
        ///--------------------------------------------------------------------------------------------------
        public static void Configure()
        {
            try
            {


                var logConfiguration = ConfigurationManager.GetSection(LogConfiguration.SECTION_NAME) as LogConfiguration;
                if (logConfiguration != null)
                {
                    LoadTiposLog(logConfiguration);

                    foreach (LoggerConf logConf in logConfiguration.Loggers)
                    {
                        if (logConf != null)
                        {
                            Type t = logConf.Type;
                            Logger l = (Logger)Activator.CreateInstance(t);
                            l.Format = logConf.Format;
                            l.Id = logConf.Id;
                            CartifLogs.RegisterLogger(l);
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                throw new FormatException("The elements in config file are bad formated", ex);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Loads tipos log. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="logConfiguration"> The log configuration. </param>
        ///--------------------------------------------------------------------------------------------------
        private static void LoadTiposLog(LogConfiguration logConfiguration)
        {
            foreach (TipoLogConf tipoLogConf in logConfiguration.TiposLog)
            {
                if (tipoLogConf != null)
                    TipoLog.RegisterTipoLog(new TipoLog(tipoLogConf.Nombre, tipoLogConf.RutaRaiz, tipoLogConf.FileFormat));
            }
        }

        const string SECTION_NAME = "CartifLogs";   /* Name of the section */
        const string TIPO_LOGS = "TiposLog";    /* The tipo logs */
        const string LOGGERS = "Loggers";   /* The loggers */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the tipos log. </summary>
        /// <value> The tipos log. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(TIPO_LOGS, IsDefaultCollection = true)]
        public TipoLogConfCollection TiposLog
        {
            get { return (TipoLogConfCollection)base[TIPO_LOGS]; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the loggers. </summary>
        /// <value> The loggers. </value>
        ///--------------------------------------------------------------------------------------------------
        [ConfigurationProperty(LOGGERS, IsDefaultCollection = true)]
        public LoggerConfCollection Loggers
        {
            get { return (LoggerConfCollection)base[LOGGERS]; }
        }
    }


}
