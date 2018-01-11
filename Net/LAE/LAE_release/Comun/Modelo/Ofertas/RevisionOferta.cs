using Cartif.Extensions;
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

namespace LAE.Comun.Modelo
{
    public static class FactoriaRevisionesOferta
    {
        public static readonly String AGUA = "Aguas";
        public static readonly String BIOMASA = "Biomasa";

        public static void LoadPuntosControl(this RevisionOferta rev)
        {
             rev.PuntosControl= PersistenceManager.SelectByProperty<PuntocontrolRevision>("IdRevision", rev.Id).ToArray();
        }

        public static bool ExisteRevisionEnviadaOAceptada(Oferta o)
        {
            String consulta = @"SELECT EXISTS(
                                    SELECT id_revisionoferta IdRevision 
                                    FROM revisiones_oferta
                                    INNER JOIN ofertas ON id_oferta=idoferta_revisionoferta
                                    WHERE (enviada_revisionoferta=true OR aceptada_revisionoferta=true)
                                    AND id_oferta=:IdOferta)";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<bool>(consulta, new { IdOferta=o.Id}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al comprobar si existen revisiones enviadas");
                return true;
            }
        }

        public static bool ExisteTipoMuestra(this RevisionOferta revision, String tipo)
        {
            String consulta = @"SELECT EXISTS(
                                        SELECT id_tipomuestrarevision
                                            FROM tipomuestra_revision
                                            INNER JOIN tipos_muestra ON idtipomuestra_tipomuestrarevision=id_tipomuestra
                                            WHERE idrevision_tipomuestrarevision=:IdRevision
                                                AND nombre_tipomuestra LIKE :NombreTipo
                                    )";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<bool>(consulta, new { IdRevision = revision.Id, NombreTipo= tipo}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al comprobar los tipos de muestra de la revisión");
                return false;
            }
        }

        public static bool ExisteTomaMuestra(this RevisionOferta revision)
        {
            String consulta = @"SELECT EXISTS(
                                        SELECT idparametro_linearevisionoferta
                                            FROM lineas_revisionoferta
                                            INNER JOIN puntocontrol_revision ON idpcontrolrevision_linearevisionoferta = id_puntocontrolrevision
                                            WHERE idrevision_puntocontrolrevision = :IdRevision
                                                AND idparametro_linearevisionoferta = :IdTomaMuestra
                                    )";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<bool>(consulta, new { IdRevision = revision.Id, IdTomaMuestra = FactoriaParametros.IDTOMAMUESTRA }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al comprobar si existe parámetro de Toma de Muestra");
                return false;
            }
        }

    }

    [TableProperties("revisiones_oferta")]
    public class RevisionOferta : PersistenceData, IModelo
    {
        [ColumnProperties("id_revisionoferta", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("num_revisionoferta")]
        public int Num { get; set; }

        [ColumnProperties("enviada_revisionoferta")]
        public bool Enviada { get; set; }

        [ColumnProperties("aceptada_revisionoferta")]
        public bool Aceptada { get; set; }

        [ColumnProperties("observaciones_revisionoferta")]
        public String Observaciones { get; set; }

        [ColumnProperties("fechaemision_revisionoferta")]
        public DateTime? FechaEmision { get; set; }

        [ColumnProperties("importe_revisionoferta")]
        public int Importe { get; set; }

        [ColumnProperties("requieretomamuestra_revisionoferta")]
        public bool? RequiereTomaMuestra { get; set; }

        [ColumnProperties("lugarmuestra_revisionoferta")]
        public String LugarMuestra { get; set; }

        [ColumnProperties("numpuntosmuestreo_revisionoferta")]
        public int? NumPuntosMuestreo { get; set; }

        [ColumnProperties("trabajopuntual_revisionoferta")]
        public bool? TrabajoPuntual { get; set; }

        [ColumnProperties("frecuencia_revisionoferta")]
        public String Frecuencia { get; set; }

        [ColumnProperties("plazorealizacion_revisionoferta")]
        public String PlazoRealizacion { get; set; }

        [ColumnProperties("idoferta_revisionoferta")]
        public int? IdOferta { get; set; }

        [ColumnProperties("idtecnico_revisionoferta")]
        public int IdTecnico { get; set; }

        public PuntocontrolRevision[] PuntosControl { get; set; }

        public string NumCodigo
        {
            get { return String.Format("{0:0#}", Num); }
            set { }
        }

        public override bool Equals(object obj)
        {
            RevisionOferta item = obj as RevisionOferta;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }
    }
}
