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
using LAE.Biomasa.Modelo;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaMuestraRecepcionBiomasa
    {
        public static int GetTecnico(int idMuestra)
        {
            String consulta = @"SELECT idtecnico_recepcionbiomasa
                                FROM recepcion_biomasa
                                INNER JOIN muestra_recepcionbiomasa ON id_recepcionbiomasa = idrecepcion_muestrarecepcionbiomasa
                                WHERE id_muestrarecepcionbiomasa = :IdMuestra";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<int>(consulta, new { IdMuestra = idMuestra }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener el técnico");
                return 0;
            }
        }

        public static MuestraRecepcionBiomasa[] GetMuestrasByProcedimiento(String siglas)
        {
            String consulta = @"SELECT id_muestrarecepcionbiomasa Id
                                FROM muestra_recepcionbiomasa
                                INNER JOIN parametros_muestrabiomasa ON id_muestrarecepcionbiomasa=idmuestra_parametromuestrabiomasa
                                INNER JOIN procedimientos ON idprocedimiento_parametromuestrabiomasa=id_procedimiento
                                WHERE siglas_procedimiento=:Siglas";
            try
            {


                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    return conn.Query<MuestraRecepcionBiomasa>(consulta, new { Siglas = siglas })
                        .Map(m =>
                        {
                            m.Load();
                            return m;
                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener las muestras");
                return new MuestraRecepcionBiomasa[0];
            }
        }
    }

    [TableProperties("muestra_recepcionbiomasa")]
    public class MuestraRecepcionBiomasa : PersistenceData, IModelo
    {
        [ColumnProperties("id_muestrarecepcionbiomasa", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("identificacion_muestrarecepcionbiomasa")]
        public String Identificacion { get; set; }

        [ColumnProperties("numcodigo_muestrarecepcionbiomasa")]
        public int NumCodigo { get; set; }

        [ColumnProperties("descripcion_muestrarecepcionbiomasa")]
        public String Descripcion { get; set; }

        [ColumnProperties("cantidad_muestrarecepcionbiomasa")]
        public decimal? Cantidad { get; set; }

        [ColumnProperties("idudscantidad_muestrarecepcionbiomasa")]
        public int? IdUdsCantidad { get; set; }

        [ColumnProperties("acreditada_muestrarecepcionbiomasa")]
        public bool Acreditada { get; set; }

        [ColumnProperties("idrecepcion_muestrarecepcionbiomasa")]
        public int IdRecepcion { get; set; }

        [ColumnProperties("idpuntocontrol_muestrarecepcionbiomasa")]
        public int IdPuntoControl { get; set; }

        public String GetCodigoLae
        {
            get
            {
                RecepcionBiomasa rec = PersistenceManager.SelectByID<RecepcionBiomasa>(IdRecepcion);
                if (rec != null)
                {
                    Trabajo t = PersistenceManager.SelectByID<Trabajo>(rec.IdTrabajo);
                    Oferta o = PersistenceManager.SelectByID<Oferta>(t.IdOferta);
                    return String.Format("{0}-SE-{1:0#}-M-3{2:000#}-{3:yy}", o.Codigo, t.NumCodigo, NumCodigo, rec.FechaRecepcion);
                }
                else
                    return null;
            }
            set { }
        }
    }
}