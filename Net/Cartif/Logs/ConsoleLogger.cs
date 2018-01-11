using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A console logger. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class ConsoleLogger : Logger
    {
        private int id; /* The identifier */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the identifier. </summary>
        /// <value> The identifier. </value>
        ///--------------------------------------------------------------------------------------------------
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String format;  /* Describes the format to use */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the format to use. </summary>
        /// <value> The format. </value>
        ///--------------------------------------------------------------------------------------------------
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public ConsoleLogger()
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Volcar logs. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipo"> The tipo. </param>
        /// <param name="logs"> The logs. </param>
        ///--------------------------------------------------------------------------------------------------
        public void VolcarLogs(TipoLog tipo, Log[] logs)
        {
            foreach (var log in logs)
                Console.WriteLine(log.ToString(Format));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Volcar log. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipo"> The tipo. </param>
        /// <param name="log">  The log. </param>
        ///--------------------------------------------------------------------------------------------------
        public void VolcarLog(TipoLog tipo, Log log)
        {
            Console.WriteLine(log.ToString(Format));
        }
    }
}
