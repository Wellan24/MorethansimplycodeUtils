using Cartif.Extensions;
using Cartif.Logs;
using Dapper;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Biomasa.Modelo
{
    public class FactoriaFusibilidadMaterialesreferencia
    {

        public static FusibilidadMaterialesreferencia[] GetMaterial(int idControl, bool? certificado = null)
        {
            String consulta = @"SELECT id_fusibilidadmaterialreferencia Id
                                    FROM biomasa.fusibilidad_materialesreferencia
                                    INNER JOIN materiales_referencia ON idmaterial_fusibilidadmaterialreferencia = id_materialreferencia
                                    WHERE (certificado_materialreferencia=@Certificado OR @Certificado IS NULL)
                                    AND usable_materialreferencia = TRUE
                                UNION
                                SELECT id_fusibilidadmaterialreferencia Id
                                    FROM biomasa.fusibilidad_materialesreferencia
                                    INNER JOIN biomasa.fusibilidad_control ON id_fusibilidadmaterialreferencia = idmaterialreferencia_fusibilidadcontrol
                                    WHERE id_fusibilidadcontrol = :IdControl";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    return conn.Query<FusibilidadMaterialesreferencia>(consulta, new { IdControl = idControl, Certificado = certificado })
                        .Map(m =>
                        {
                            m.Load(conn);
                            return m;
                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los material de referencia. Por favor, recargue la página o informa a soporte.");
                return new FusibilidadMaterialesreferencia[0];
            }
        }
    }

    [TableProperties("biomasa.fusibilidad_materialesreferencia")]
    public class FusibilidadMaterialesreferencia : PersistenceData
    {
        [ColumnProperties("id_fusibilidadmaterialreferencia", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("temperatura_fusibilidadmaterialreferencia")]
        public double? Temperatura { get; set; }

        [ColumnProperties("idudstemperatura_fusibilidadmaterialreferencia")]
        public int? IdUdsTemperatura { get; set; }

        [ColumnProperties("idmaterial_fusibilidadmaterialreferencia")]
        public int? IdMaterial { get; set; }

        public override string ToString()
        {
            MaterialReferencia material = PersistenceManager.SelectByID<MaterialReferencia>(IdMaterial);
            String certificado = (material.Certificado == true) ? "C" : "";
            return String.Format("MR{0}-B-{1:0000}-{2:yy}", certificado, material.Codigo, material.FechaRecepcion);
        }
    }
}