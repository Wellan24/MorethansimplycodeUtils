using Cartif.Logs;
using Dapper;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Modelo
{
    public class FactoriaChnMaterialesReferencia
    {
        // TODO Rellenar esto con Selects necesarias.
        public static ChnMaterialReferencia[] GetMaterial(bool? certificado=null)
        {
            String consulta = @"SELECT id_chnmaterialreferencia Id, porcentajec_chnmaterialreferencia PorcentajeC, idudsporcentajec_chnmaterialreferencia IdUdsPorcentajeC,
                                        porcentajeh_chnmaterialreferencia PorcentajeH, idudsporcentajeh_chnmaterialreferencia IdUdsPorcentajeH,
                                        porcentajen_chnmaterialreferencia PorcentajeN, idudsporcentajen_chnmaterialreferencia IdUdsPorcentajeN, idmaterial_chnmaterialreferencia IdMaterial
                                    FROM biomasa.chn_materialesreferencia
                                    INNER JOIN materiales_referencia ON idmaterial_chnmaterialreferencia = id_materialreferencia
                                    WHERE certificado_materialreferencia=@Certificado OR @Certificado IS NULL";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<ChnMaterialReferencia>(consulta.ToString(), new { Certificado = certificado }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los material de referencia. Por favor, recargue la página o informa a soporte.");
                return new ChnMaterialReferencia[0];
            }
        }
    }

    [TableProperties("biomasa.chn_materialesreferencia")]
    public class ChnMaterialReferencia : PersistenceData
    {
        [ColumnProperties("id_chnmaterialreferencia", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("porcentajec_chnmaterialreferencia")]
        public double? PorcentajeC { get; set; }

        [ColumnProperties("idudsporcentajec_chnmaterialreferencia")]
        public int? IdUdsPorcentajeC { get; set; }

        [ColumnProperties("porcentajeh_chnmaterialreferencia")]
        public double? PorcentajeH { get; set; }

        [ColumnProperties("idudsporcentajeh_chnmaterialreferencia")]
        public int? IdUdsPorcentajeH { get; set; }

        [ColumnProperties("porcentajen_chnmaterialreferencia")]
        public double? PorcentajeN { get; set; }

        [ColumnProperties("idudsporcentajen_chnmaterialreferencia")]
        public int? IdUdsPorcentajeN { get; set; }

        [ColumnProperties("idmaterial_chnmaterialreferencia")]
        public int? IdMaterial { get; set; }

        public MaterialReferencia Material { get; set; }

        public override string ToString()
        {
            MaterialReferencia material = PersistenceManager.SelectByID<MaterialReferencia>(IdMaterial);
            String certificado = (material.Certificado == true) ? "C" : "";
            return String.Format("MR{0}-B-{1:0000}-{2:yy}", certificado, material.Codigo, material.FechaRecepcion);
        }
    }
}