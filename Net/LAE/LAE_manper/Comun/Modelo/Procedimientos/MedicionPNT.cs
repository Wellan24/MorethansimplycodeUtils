using Cartif.Logs;
using Dapper;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LAE.Comun.Modelo;

namespace LAE.Comun.Modelo.Procedimientos
{
    public class FactoriaMedicionPNT
    {

        public static MedicionPNT GetDefault(int idTecnico, int idMuestra, bool addFechaFin = false)
        {
            return new MedicionPNT() { FechaInicio = DateTime.Now, IdTecnico = idTecnico, IdMuestra = idMuestra };
        }

        public static MedicionPNT[] GetDefaults(int idTecnico, int idMuestra, bool addFechaFin = false)
        {
            return new MedicionPNT[] { new MedicionPNT { FechaInicio = DateTime.Now, IdTecnico = idTecnico, IdMuestra = idMuestra } };
        }

        public static MedicionPNT GetMedicion(int idMuestra, String tabla, String id)
        {
            StringBuilder consulta = new StringBuilder(@"SELECT id_medicionpnt Id, fechainicio_medicionpnt FechaInicio, idtecnico_medicionpnt IdTecnico, observaciones_medicionpnt Observaciones, idmuestra_medicionpnt IdMuestra, finalizado_medicionpnt Finalizado
                                    FROM medicion_pnt
                                    LEFT JOIN medicion_pntcci ON id_medicionpnt = idcci_medicionpntcci
                                    INNER JOIN ");
            consulta.Append(tabla);
            consulta.Append(" ON id_medicionpnt=");
            consulta.Append(id);
            consulta.Append(" WHERE idcci_medicionpntcci is null AND idmuestra_medicionpnt = :IdMuestra");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    MedicionPNT medicion = conn.Query<MedicionPNT>(consulta.ToString(), new { IdMuestra = idMuestra }).FirstOrDefault();
                    return medicion;
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener la medición. Por favor, recargue la página o informa a soporte.");
                return null;
            }

        }

        public static MedicionPNT[] GetMediciones(int idMuestra, String tabla, String id)
        {
            StringBuilder consulta = new StringBuilder(@"SELECT id_medicionpnt Id, fechainicio_medicionpnt FechaInicio, idtecnico_medicionpnt IdTecnico, observaciones_medicionpnt Observaciones, idmuestra_medicionpnt IdMuestra, finalizado_medicionpnt Finalizado
                                    FROM medicion_pnt
                                    LEFT JOIN medicion_pntcci ON id_medicionpnt=idcci_medicionpntcci
                                    INNER JOIN ");
            consulta.Append(tabla);
            consulta.Append(" ON id_medicionpnt=");
            consulta.Append(id);
            consulta.Append(" WHERE idcci_medicionpntcci is null AND idmuestra_medicionpnt = :IdMuestra");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    MedicionPNT[] mediciones = conn.Query<MedicionPNT>(consulta.ToString(), new { IdMuestra = idMuestra }).ToArray();
                    return (mediciones.Count() > 0) ? mediciones : null;
                }

            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener la medición. Por favor, recargue la página o informa a soporte.");
                return null;
            }
        }
    }

    [TableProperties("medicion_pnt")]
    public class MedicionPNT : PersistenceData, IModelo
    {
        [ColumnProperties("id_medicionpnt", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("fechainicio_medicionpnt")]
        public DateTime FechaInicio { get; set; }

        [ColumnProperties("idtecnico_medicionpnt")]
        public int IdTecnico { get; set; }

        [ColumnProperties("observaciones_medicionpnt")]
        public String Observaciones { get; set; }

        [ColumnProperties("idmuestra_medicionpnt")]
        public int IdMuestra { get; set; }

        [ColumnProperties("finalizado_medicionpnt")]
        public Boolean Finalizado { get; set; }
    }
}