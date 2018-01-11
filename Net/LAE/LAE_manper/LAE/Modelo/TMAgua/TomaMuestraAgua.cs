using Cartif.Extensions;
using Cartif.Logs;
using Dapper;
using LAE.Clases;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LAE.Comun.Modelo;

namespace LAE.Modelo
{
    public static class FactoriaTomaMuestraAgua
    {
        public static void LoadData(this TomaMuestraAgua tma)
        {
            PersistenceDataManipulation.LoadData("IdTomaMuestra", tma.Id, tma.BlancosMuestreo);
        }

        public static bool LastNumBlanco(this TomaMuestraAgua tma)
        {
            String consulta = @"SELECT toma1.numblancomuestreo_tomamuestraagua num
                                    FROM tomamuestra_agua toma1
                                    INNER JOIN trabajos traba1 ON toma1.idtrabajo_tomamuestraagua = traba1.id_trabajo
                                    INNER JOIN ofertas oferta1 ON traba1.idoferta_trabajo = oferta1.id_oferta
                                    INNER JOIN ofertas oferta2 ON EXTRACT(year FROM oferta1.anno_oferta) = EXTRACT(year FROM oferta2.anno_oferta)
                                    INNER JOIN trabajos traba2 ON oferta2.id_oferta = traba2.idoferta_trabajo
                                    INNER JOIN tomamuestra_agua toma2 ON traba2.id_trabajo = toma2.idtrabajo_tomamuestraagua
                                    WHERE toma2.id_tomamuestraagua = @Id and toma1.numblancomuestreo_tomamuestraagua IS NOT NULL ORDER BY num DESC LIMIT 1";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    int? lastBlanco = conn.Query<int?>(consulta, new { Id = tma.Id }).FirstOrDefault();
                    return (lastBlanco == tma.NumBlanco);
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al comprobar el borrado del blanco de muestreo. Por favor, recargue la página o informa a soporte.");
                return false;
            }
        }

    }

    [TableProperties("tomamuestra_agua")]
    public class TomaMuestraAgua : PersistenceData, IModelo
    {
        [ColumnProperties("id_tomamuestraagua", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("numcodigo_tomamuestraagua")]
        public int NumCodigo { get; set; }

        [ColumnProperties("numblancomuestreo_tomamuestraagua")]
        public int? NumBlanco { get; set; }

        [ColumnProperties("idtrabajo_tomamuestraagua")]
        public int IdTrabajo { get; set; }

        public List<BlancomuestreoTMAgua> BlancosMuestreo;
        /* no hay lista para muestras (porque guardo los datos en el control ControlMuestraAgua) */

        public String GetCodigoBlanco
        {
            get
            {
                Trabajo t = PersistenceManager.SelectByID<Trabajo>(IdTrabajo);
                Oferta o = PersistenceManager.SelectByID<Oferta>(t.IdOferta);
                return String.Format("BM-{0}/{1:yy}", (NumBlanco != null ? String.Format("{0:0#}", NumBlanco) : "XX"), o.AnnoOferta);
            }
            set { }
        }
    }
}
