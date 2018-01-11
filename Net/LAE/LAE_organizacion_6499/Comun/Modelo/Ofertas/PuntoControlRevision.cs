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

namespace LAE.Comun.Modelo
{
    public static class FactoriaPuntocontrolRevision
    {
        public static void LoadLineasRevision(this PuntocontrolRevision pc)
        {
            pc.LineasRevision = PersistenceManager.SelectByProperty<LineaRevisionOferta>("IdPControlRevisionOferta", pc.Id).ToArray();
        }

        public static PuntocontrolRevision[] GetPuntosControl(int idTrabajo)
        {
            String consulta = @"SELECT id_puntocontrolrevision Id, nombre_puntocontrolrevision Nombre, observaciones_puntocontrolrevision Observaciones, importe_puntocontrolrevision Importe, idrevision_puntocontrolrevision IdRevision
                                    FROM puntocontrol_revision
                                    INNER JOIN revisiones_oferta on idrevision_puntocontrolrevision = id_revisionoferta
                                    INNER JOIN ofertas on idoferta_revisionoferta = id_oferta
                                    INNER JOIN trabajos on id_oferta = idoferta_trabajo
                                    WHERE id_trabajo = :IdTrabajo AND aceptada_revisionoferta=true";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<PuntocontrolRevision>(consulta, new { IdTrabajo = idTrabajo }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener los datos de parámetros");
                return new PuntocontrolRevision[0];
            }
        }

        public static PuntocontrolRevision[] GetPuntosControlConTomaMuestra(int idTrabajo)
        {
            String consulta = @"SELECT id_puntocontrolrevision Id, nombre_puntocontrolrevision Nombre, observaciones_puntocontrolrevision Observaciones, importe_puntocontrolrevision Importe, idrevision_puntocontrolrevision IdRevision
                                        FROM puntocontrol_revision
                                        INNER JOIN revisiones_oferta on idrevision_puntocontrolrevision = id_revisionoferta
                                        INNER JOIN ofertas on idoferta_revisionoferta = id_oferta
                                        INNER JOIN trabajos on id_oferta = idoferta_trabajo
                                        WHERE id_trabajo = :IdTrabajo 
                                        AND aceptada_revisionoferta=true
                                        AND id_puntocontrolrevision IN (SELECT id_puntocontrolrevision
                                                 FROM puntocontrol_revision
                                                 INNER JOIN lineas_revisionoferta ON id_puntocontrolrevision = idpcontrolrevision_linearevisionoferta
                                                 WHERE idparametro_linearevisionoferta = :IdTomaMuestra)";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<PuntocontrolRevision>(consulta, new { IdTrabajo = idTrabajo, IdTomaMuestra = FactoriaParametros.IDTOMAMUESTRA }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener los datos de parámetros");
                return new PuntocontrolRevision[0];
            }
        }
    }

    [TableProperties("puntocontrol_revision")]
    public class PuntocontrolRevision : PersistenceData
    {
        [ColumnProperties("id_puntocontrolrevision", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_puntocontrolrevision")]
        public String Nombre { get; set; }

        [ColumnProperties("observaciones_puntocontrolrevision")]
        public String Observaciones { get; set; }

        [ColumnProperties("importe_puntocontrolrevision")]
        public decimal Importe { get; set; }

        [ColumnProperties("idrevision_puntocontrolrevision")]
        public int IdRevision { get; set; }

        public LineaRevisionOferta[] LineasRevision { get; set; }

        //public override bool Equals(object obj)
        //{
        //    PuntocontrolRevision item = obj as PuntocontrolRevision;
        //    if (item != null)
        //        return item.Id.Equals(Id);
        //    return false;

        //}
    }
}
