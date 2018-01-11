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
    public class FactoriaMaterialPNT
    {
        public static MaterialPNT[] GetMaterial(int idParametro)
        {
            String consulta = @"SELECT id_materialpnt Id, nombre_materialpnt Nombre, capacidad_materialpnt Capacidad, idudscapacidad_materialpnt IdUdsCapacidad
                                    FROM material_pnt
                                    INNER JOIN parametro_materialpnt ON id_materialpnt=idmaterial_parametromaterialpnt
                                    WHERE idparametro_parametromaterialpnt = :IdParametro
                                    ORDER BY id_materialpnt DESC";
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<MaterialPNT>(consulta, new { IdParametro = idParametro }).ToArray();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los materiales usados en el análisis. Por favor, recargue la página o informa a soporte.");
                return null;
            }
        }

    }

    [TableProperties("material_pnt")]
    public class MaterialPNT : PersistenceData
    {
        [ColumnProperties("id_materialpnt", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_materialpnt")]
        public String Nombre { get; set; }

        [ColumnProperties("capacidad_materialpnt")]
        public double? Capacidad { get; set; }

        [ColumnProperties("idudscapacidad_materialpnt")]
        public int? IdUdsCapacidad { get; set; }

        public override string ToString()
        {
            return Nombre + "-" + Capacidad +" "+ PersistenceManager.SelectByID<Unidad>(IdUdsCapacidad).Abreviatura;
        }
    }
}