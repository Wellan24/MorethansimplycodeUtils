using Cartif.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A cartif logs. </summary>
    /// <remarks> Oscvic, 2016-01-11. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class CartifLogs
    {
        private static CartifLogger cartifLogger;   /* The cartif logger */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Static constructor. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        ///--------------------------------------------------------------------------------------------------
        static CartifLogs()
        {
            cartifLogger = new CartifLogger();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Starts a logging. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public static void Configure()
        {
            LogConfiguration.Configure();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Registers the logger described by l. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="l"> The Logger to process. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void RegisterLogger(Logger l)
        {
            cartifLogger.RegisterLogger(l);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y si el numero de logs en cola es mayor de 5 se vuelcan al
        ///           archivo. Si inmediato es true, se hace el volcado automáticamente. Esto es indicado
        ///           para try catch. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog">   Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="log">       El mensaje a mostrar. </param>
        /// <param name="exception"> Boolean para realizar el volcado automático. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void GenerarLog(TipoLog tipoLog, String log, Exception exception)
        {
            cartifLogger.GenerarLog(tipoLog, new StackTrace().GetFrame(1).GetMethod(), log, exception, true);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y si el numero de logs en cola es mayor de 5 se vuelcan al
        ///           archivo. Si inmediato es true, se hace el volcado automáticamente. Esto es indicado
        ///           para try catch. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog">   Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="log">       El mensaje a mostrar. </param>
        /// <param name="inmediato"> Boolean para realizar el volcado automático. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void GenerarLog(TipoLog tipoLog, String log, bool inmediato)
        {
            cartifLogger.GenerarLog(tipoLog, new StackTrace().GetFrame(1).GetMethod(), log, null, inmediato);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y si el numero de logs en cola es mayor de 5 se vuelcan al
        ///           archivo. Si inmediato es true, se hace el volcado automáticamente. Esto es indicado
        ///           para try catch. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog"> Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="log">     El mensaje a mostrar. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void GenerarLog(TipoLog tipoLog, String log)
        {
            cartifLogger.GenerarLog(tipoLog, new StackTrace().GetFrame(1).GetMethod(), log, null, false);
        }
    }

}