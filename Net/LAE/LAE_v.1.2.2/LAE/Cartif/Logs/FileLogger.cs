using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace Cartif.Logs
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A file logger. </summary>
    /// <remarks> Oscvic, 2016-01-11. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class FileLogger : Logger
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
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-12. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public FileLogger()
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
            FileInfo archivo = GetFileInfoArchivoLog(tipo);

            using (StreamWriter stream = archivo.AppendText())
            {
                foreach (var log in logs)
                    stream.WriteLine(log.ToString(format));
            }

        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Volcar log. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipo"> The tipo. </param>
        /// <param name="log">  The log. </param>
        ///--------------------------------------------------------------------------------------------------
        public void VolcarLog(TipoLog tipo, Log log)
        {
            FileInfo archivo = GetFileInfoArchivoLog(tipo);

            using (StreamWriter stream = archivo.AppendText())
                stream.WriteLine(log.ToString(format));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets file information archivo log. </summary>
        /// <remarks> Oscvic, 2016-01-11. </remarks>
        /// <param name="tipo"> The tipo. </param>
        /// <returns> The file information archivo log. </returns>
        ///--------------------------------------------------------------------------------------------------
        private FileInfo GetFileInfoArchivoLog(TipoLog tipo)
        {
            DateTime fecha = DateTime.Now;
            String raiz = tipo.RutaRaiz.EndsWith("/") ? tipo.RutaRaiz : tipo.RutaRaiz + "/";
            FileInfo archivo = new FileInfo(raiz + String.Format(tipo.FileFormat, tipo.Nombre, fecha));
            archivo.Directory.Create();
            return archivo;
        }
    }
}
