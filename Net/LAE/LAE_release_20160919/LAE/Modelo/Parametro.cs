using Cartif.Logs;
using Dapper;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Modelo
{
    public class FactoriaParametros
    {
        public static readonly int IDTOMAMUESTRA = 1;
        /* PARA GENERAR DOCUMENTACIÓN */

        //TODO necesito reparar (lineas_revisionoferta)
        public static IEnumerable<KeyValuePair<Parametro, int>> GetParametrosRevisionPorMuestra(RevisionOferta rev, TipoMuestra tipo = null)
        {

            String consulta = @"SELECT nombre_parametro NombreParametro, metodo_parametro MetodoParametro, idtipomuestra_parametro IdTipoMuestra, 
                                                                    norma_parametro Norma, cantidad_linearevisionoferta Item1
                                                            FROM parametros
                                                            INNNER JOIN tipos_muestra ON idtipomuestra_parametro = id_tipomuestra
                                                            INNER JOIN lineas_revisionoferta ON id_parametro = idparametro_linearevisionoferta
                                                            WHERE idrevisionoferta_linearevisionoferta = :RevId
                                                            AND (:TipoId IS NULL OR id_tipomuestra = :TipoId)
                                                            ORDER BY MetodoParametro, Norma, NombreParametro";
            Func<Parametro, Tuple<int>, KeyValuePair<Parametro, int>> map = (p, t) => new KeyValuePair<Parametro, int>(p, t.Item1);
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query(consulta, map, new { RevId = rev.Id, TipoId = tipo?.Id }, splitOn: "Item1");
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los parámetros. Por favor, recargue la página o informa a soporte.");
                return Enumerable.Empty<KeyValuePair<Parametro, int>>();
            }
        }

        public static IEnumerable<Tuple<Parametro, int, PuntocontrolRevision>> GetParametrosRevisionPorMuestra2(RevisionOferta rev, TipoMuestra tipo = null)
        {

            String consulta = @"SELECT nombre_parametro NombreParametro, metodo_parametro MetodoParametro, idtipomuestra_parametro IdTipoMuestra, 
                                                                    norma_parametro Norma, cantidad_linearevisionoferta Item1, id_puntocontrolrevision Id, nombre_puntocontrolrevision Nombre, observaciones_puntocontrolrevision Observaciones, importe_puntocontrolrevision Importe
                                                            FROM parametros
                                                            INNNER JOIN tipos_muestra ON idtipomuestra_parametro = id_tipomuestra
                                                            INNER JOIN lineas_revisionoferta ON id_parametro = idparametro_linearevisionoferta
                                                            INNER JOIN puntocontrol_revision ON idpcontrolrevision_linearevisionoferta=id_puntocontrolrevision
                                                            WHERE idrevision_puntocontrolrevision = :RevId
                                                            AND (:TipoId IS NULL OR id_tipomuestra = :TipoId)
                                                            ORDER BY id_puntocontrolrevision";
            Func<Parametro, Tuple<int>, PuntocontrolRevision, Tuple<Parametro, int, PuntocontrolRevision>> map = (p, t, pc) => new Tuple<Parametro, int, PuntocontrolRevision>(p, t.Item1, pc);
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query(consulta, map, new { RevId = rev.Id, TipoId = tipo?.Id }, splitOn: "Item1, Id");
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los parámetros. Por favor, recargue la página o informa a soporte.");
                return Enumerable.Empty<Tuple<Parametro, int, PuntocontrolRevision>>();
            }
        }

        public static IEnumerable<KeyValuePair<Parametro, int>> GetParametrosPeticionPorMuestra(Peticion pet, TipoMuestra tipo = null)
        {

            String consulta = @"SELECT nombre_parametro NombreParametro, metodo_parametro MetodoParametro, idtipomuestra_parametro IdTipoMuestra, 
                                                                    norma_parametro Norma, cantidad_lineapeticion Item1
                                                            FROM parametros
                                                            INNNER JOIN tipos_muestra ON idtipomuestra_parametro = id_tipomuestra
                                                            INNER JOIN lineas_peticion ON id_parametro = idparametro_lineapeticion
                                                            WHERE idpeticion_lineapeticion = :PetId
                                                            AND (:TipoId IS NULL OR id_tipomuestra = :TipoId)
                                                            ORDER BY MetodoParametro, Norma, NombreParametro";
            Func<Parametro, Tuple<int>, KeyValuePair<Parametro, int>> map = (p, t) => new KeyValuePair<Parametro, int>(p, t.Item1);
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query(consulta, map, new { PetId = pet.Id, TipoId = tipo?.Id }, splitOn: "Item1");
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los parámetros. Por favor, recargue la página o informa a soporte.");
                return Enumerable.Empty<KeyValuePair<Parametro, int>>();
            }
        }

        public static Parametro[] GetParametrosByTipo(String tipo)
        {
            int idTipo = PersistenceManager.SelectByProperty<TipoMuestra>("Nombre", tipo).FirstOrDefault()?.Id ?? 0;
            return PersistenceManager.SelectByProperty<Parametro>("IdTipoMuestra", idTipo).ToArray();

        }


        /* PARA GENERAR INTERFAZ */
        public static IEnumerable<Parametro> ParametrosOrdenados()
        {

            StringBuilder consulta = new StringBuilder(@"SELECT id_parametro Id, nombre_parametro NombreParametro, metodo_parametro MetodoParametro, idtipomuestra_parametro IdTipoMuestra, norma_parametro Norma
                                                            FROM parametros
                                                            INNNER JOIN tipos_muestra ON idtipomuestra_parametro = id_tipomuestra
                                                            ORDER BY nombre_tipomuestra, MetodoParametro, Norma, NombreParametro");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<Parametro>(consulta.ToString());
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al obtener los parámetros. Por favor, recargue la página o informa a soporte.");
                return null;
            }
        }
    }

    [TableProperties("parametros")]
    public class Parametro : PersistenceData, IModelo, IComparable<Parametro>
    {
        [ColumnProperties("id_parametro", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("nombre_parametro")]
        public String NombreParametro { get; set; }

        [ColumnProperties("metodo_parametro")]
        public String MetodoParametro { get; set; }

        [ColumnProperties("idtipomuestra_parametro")]
        public int IdTipoMuestra { get; set; }

        [ColumnProperties("norma_parametro")]
        public String Norma { get; set; }

        [ColumnProperties("conservacion_parametro")]
        public String Conservacion { get; set; }

        public override bool Equals(object obj)
        {
            Parametro item = obj as Parametro;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }

        public override String ToString()
        {
            return ((MetodoParametro == null) ? "" : MetodoParametro + " ") + NombreParametro + ((Norma == null) ? "" : " (" + Norma + ")");
        }

        public int CompareTo(Parametro other)
        {
            try
            {
                /*Comparar por metodo*/
                if (this.MetodoParametro == null && other.MetodoParametro != null)
                    return 1;
                else if (this.MetodoParametro != null && other.MetodoParametro == null)
                    return -1;
                else if (this.MetodoParametro != null && other.MetodoParametro != null)
                {
                    /*Comprobar que tiene formato PNT-LAE-x-x*/
                    Regex rgx = new Regex(@"^PNT-LAE-\d+-\d+$"); /*No tocar patron, afecta al split linea 98 y 99 */
                    if (rgx.IsMatch(this.MetodoParametro) && rgx.IsMatch(other.MetodoParametro))
                    {
                        String[] thisParam = this.MetodoParametro.Split('-');
                        String[] otherParam = other.MetodoParametro.Split('-');

                        int compareMetodNum1 = (int.Parse(thisParam[2])).CompareTo(int.Parse(otherParam[2]));
                        if (compareMetodNum1 != 0)
                            return compareMetodNum1;
                        else
                            return (int.Parse(thisParam[3])).CompareTo(int.Parse(otherParam[3]));
                    }
                }

                /*Comparar por Norma */
                if (this.Norma == null && other.Norma != null && other.Norma.Contains("UNE"))
                    return 1;
                else if (other.Norma == null && this.Norma != null && this.Norma.Contains("UNE"))
                    return -1;
                else if (this.Norma != null && other.Norma != null)
                {
                    if (!this.Norma.Contains("UNE") && other.Norma.Contains("UNE"))
                        return 1;
                    else if (this.Norma.Contains("UNE") && !other.Norma.Contains("UNE"))
                        return -1;
                    else if (this.Norma.Contains("UNE") && other.Norma.Contains("UNE"))
                        return this.Norma.CompareTo(other.Norma);
                }
                /*Compara por Nombre*/
                return this.NombreParametro.CompareTo(other.NombreParametro);
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al ordenar los datos", ex);
                MessageBox.Show("Se ha producido un error al ordenar los parámetros. Por favor, informa a soporte.");
                return 0;
            }
        }

    }
}
