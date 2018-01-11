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
    public class FactoriaTipoMuestra
    {
        public static IEnumerable<TipoMuestra> GetMuestrasRevision(RevisionOferta rev)
        {
            StringBuilder consulta = new StringBuilder(@"SELECT id_tipomuestra Id, nombre_tipomuestra Nombre
                                                            FROM tipos_muestra
                                                            INNER JOIN tipomuestra_revision ON id_tipomuestra=idtipomuestra_tipomuestrarevision");

            consulta.Append(" WHERE idrevision_tipomuestrarevision=").Append(rev.Id);
            consulta.Append(" ORDER BY nombre_tipomuestra");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<TipoMuestra>(consulta.ToString());
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los tipos de muestra de la revisión. Por favor, recargue la página o informa a soporte.");
                return Enumerable.Empty<TipoMuestra>();
            }
        }

        public static IEnumerable<TipoMuestra> GetMuestrasPeticion(Peticion pet)
        {
            StringBuilder consulta = new StringBuilder(@"SELECT id_tipomuestra Id, nombre_tipomuestra Nombre
                                                            FROM tipos_muestra
                                                            INNER JOIN tipomuestra_peticion ON id_tipomuestra=idtipomuestra_tipomuestrapeticion");

            consulta.Append(" WHERE idpeticion_tipomuestrapeticion=").Append(pet.Id);
            consulta.Append(" ORDER BY nombre_tipomuestra");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<TipoMuestra>(consulta.ToString());
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los tipos de muestra de la petición. Por favor, recargue la página o informa a soporte.");
                return Enumerable.Empty<TipoMuestra>();
            }
        }

    }

    [TableProperties("tipos_muestra")]
    public class TipoMuestra : PersistenceData, IComparable<TipoMuestra>
    {
        [ColumnProperties("id_tipomuestra", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_tipomuestra")]
        public String Nombre { get; set; }

        public override String ToString()
        {
            return Nombre;
        }

        public int CompareTo(TipoMuestra other)
        {
            return this.Nombre.CompareTo(other.Nombre);
        }

    }
}
