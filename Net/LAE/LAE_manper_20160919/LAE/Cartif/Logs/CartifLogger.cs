using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cartif.Collections;
using Cartif.Extensions;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A cartif logger. </summary>
    /// <remarks> Oscvic, 2016-01-11. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class CartifLogger
    {
        // Candidatos a pasar a la configuracion
        public static readonly String RaizLogs = "C:/Logs/";    /* . */
        private static readonly String RutaLogs = "{0}/{1}/{2}/Día {3}.txt";    /* The ruta logs */

        private ConcurrentCartifDictionary<TipoLog, ConcurrentQueue<Log>> ColaLogs = new ConcurrentCartifDictionary<TipoLog, ConcurrentQueue<Log>>();   /* The cola logs */
        private List<Logger> loggers;   /* The loggers */
        private int NumeroLogs = 0; /* The numero logs */
        private Thread HiloLogs;    /* The hilo logs */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public CartifLogger()
        {
            // TODO leer config 
            loggers = new List<Logger>();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => VolcarLogs();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds a logger. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="logger"> The logger. </param>
        ///--------------------------------------------------------------------------------------------------
        public void RegisterLogger(Logger logger) { loggers.Add(logger); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y se inicia el volcado inmediatamente. Esto es indicado
        ///           para try catch. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog">   Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="log">       El mensaje a mostrar. </param>
        /// <param name="exception"> Boolean para realizar el volcado automático. </param>
        ///--------------------------------------------------------------------------------------------------
        public void GenerarLog(TipoLog tipoLog, String log, Exception exception)
        {
            GenerarLog(tipoLog, new StackTrace().GetFrame(1).GetMethod(), log, exception, true);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y si el numero de logs en cola es mayor de 5 se vuelcan al
        ///           archivo. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog"> Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="log">     El mensaje a mostrar. </param>
        ///--------------------------------------------------------------------------------------------------
        public void GenerarLog(TipoLog tipoLog, String log)
        {
            GenerarLog(tipoLog, new StackTrace().GetFrame(1).GetMethod(), log, null, false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y si el numero de logs en cola es mayor de 5 se vuelcan al
        ///           archivo. Si inmediato es true, se hace el volcado automáticamente. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog">     Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="log">         El mensaje a mostrar. </param>
        /// <param name="esInmediato"> Boolean para realizar el volcado automático. </param>
        ///--------------------------------------------------------------------------------------------------
        public void GenerarLog(TipoLog tipoLog, String log, bool esInmediato)
        {
            GenerarLog(tipoLog, new StackTrace().GetFrame(1).GetMethod(), log, null, esInmediato);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Añade un log a la cola de logs y si el numero de logs en cola es mayor de 5 se vuelcan al
        ///           archivo. Si inmediato es true, se hace el volcado automáticamente. Esto es indicado
        ///           para try catch. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipoLog">     Un objeto TipoLog que indica la aplicacion que lo hace. </param>
        /// <param name="method">      Para indicar donde está el log (método y/o linea) </param>
        /// <param name="log">         El mensaje a mostrar. </param>
        /// <param name="exception">   Boolean para realizar el volcado automático. </param>
        /// <param name="esInmediato"> Boolean para realizar el volcado automático. </param>
        ///--------------------------------------------------------------------------------------------------
        public void GenerarLog(TipoLog tipoLog, MethodBase method, String log, Exception exception, bool esInmediato)
        {
            try
            {
                if (tipoLog != null)
                {
                    ConcurrentQueue<Log> cola = ColaLogs[tipoLog];

                    if (cola == null)
                        ColaLogs[tipoLog] = new ConcurrentQueue<Log>();

                    ColaLogs[tipoLog].Enqueue(new Log(tipoLog, method, log, exception));
                    NumeroLogs++;

                    IniciarVolcado(esInmediato);
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("Logs"), "Problema generando logs. TipoLog=" + tipoLog.Nombre + " log=" + log, ex);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Iniciar volcado. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="inmediato"> Boolean para realizar el volcado automático. </param>
        ///--------------------------------------------------------------------------------------------------
        private void IniciarVolcado(bool inmediato)
        {
            if ((NumeroLogs > 5 || inmediato) && (HiloLogs == null || (HiloLogs != null && !HiloLogs.IsAlive)))
            {
                NumeroLogs = 0;
                HiloLogs = new Thread(new ThreadStart(VolcarLogs));
                HiloLogs.Start();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Vuelca los logs por orden en los archivos asignados a cada TipoLog. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void VolcarLogs()
        {
            try
            {
                TipoLog[] keys = ColaLogs.Keys.ToArray();

                foreach (TipoLog key in keys)
                {
                    ConcurrentQueue<Log> logsQueue = ColaLogs[key];

                    /* Obtiene todos los logs que hay que volcar */
                    List<Log> logs = new List<Log>(logsQueue.Count);
                    Log l = null;
                    while (logsQueue.TryDequeue(out l))
                        logs.Add(l);

                    /* Procesa todos logs con todos los Logger que existen */
                    Log[] logsArray = logs.ToArray();
                    loggers.OrderBy(logger => logger.Id).ForEach(logger => logger.VolcarLogs(key, logsArray));
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("Logs"), "**Excepcion** Problema volcando logs **Mensaje**" + ex.Message, true);
            }
        }
    }
}
