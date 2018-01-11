using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Interface for logger. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public interface Logger
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the identifier. </summary>
        /// <value> The identifier. </value>
        ///--------------------------------------------------------------------------------------------------
        int Id { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the format to use. </summary>
        /// <value> The format. </value>
        ///--------------------------------------------------------------------------------------------------
        String Format { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Volcar logs. </summary>
        /// <param name="tipo"> The tipo. </param>
        /// <param name="logs"> The logs. </param>
        ///--------------------------------------------------------------------------------------------------
        void VolcarLogs(TipoLog tipo, Log[] logs);

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Volcar log. </summary>
        /// <param name="tipo"> The tipo. </param>
        /// <param name="log">  The log. </param>
        ///--------------------------------------------------------------------------------------------------
        void VolcarLog(TipoLog tipo, Log log);
    }
}
