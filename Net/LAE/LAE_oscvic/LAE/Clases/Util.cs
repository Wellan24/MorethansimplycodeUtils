using Cartif.Extensions;
using Cartif.Logs;
using Dapper;
using LAE.Modelo;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAE.Clases
{
    class Util
    {
        public static List<KeyValuePair<int, string>> ComboBoxCodigoOfertas()
        {

            StringBuilder consulta = new StringBuilder(@"SELECT id_revisionoferta as Key, right(anno_oferta::text, -2) || '-' || numcodigo_oferta || '/' || num_revisionoferta as Value
                                    from ofertas
                                    inner join revisiones_oferta on id_oferta = idoferta_revisionoferta
                                    where anulada_oferta = false
                                    order by anno_oferta desc, numcodigo_oferta desc, num_revisionoferta");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<KeyValuePair<int, String>>(consulta.ToString()).ToList();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener los códigos de las ofertas");
                return null;
            }
        }

        public static Oferta GetOfertaFromRevision(int idRevision)
        {
            StringBuilder consulta = new StringBuilder(@"SELECT idcliente_oferta IdCliente, idcontacto_oferta IdContacto
                                                            from ofertas
                                                            inner join revisiones_oferta on id_oferta=idoferta_revisionoferta
                                                             where ");
            consulta.AppendFormat("id_revisionoferta={0}", idRevision);
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<Oferta>(consulta.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener la oferta de la revisión");
                return null;
            }
        }

        public static Peticion GenerarPeticionFromRevision(RevisionOferta rev, Oferta o)
        {
            Peticion p = new Peticion
            {
                RequiereTomaMuestra = rev.RequiereTomaMuestra,
                LugarMuestra = rev.LugarMuestra,
                NumPuntosMuestreo = rev.NumPuntosMuestreo,
                TrabajoPuntual = rev.TrabajoPuntual,
                Frecuencia = rev.Frecuencia,
                PlazoRealizacion = rev.PlazoRealizacion,
                Observaciones = rev.Observaciones,
                IdCliente = o.IdCliente,
                IdContacto = o.IdContacto,
                IdTecnico = rev.IdTecnico
            };
            p.Fecha = DateTime.Now;
            return p;
        }

        public static ObservableCollection<ITipoMuestra> GetTiposMuestraFromRevision(int idRevision)
        {
            ObservableCollection<ITipoMuestra> lista = new ObservableCollection<ITipoMuestra>();

            PersistenceManager.SelectByProperty<TipoMuestraRevision>("IdRevision", idRevision)
                .ForEach(l => lista.Add(
                    new ITipoMuestra
                    {
                        IdTipoMuestra = l.IdTipoMuestra
                    })
                );
            return lista;
        }

        public static ObservableCollection<ILineasParametros> GetParametrosFromRevision(int idRevision)
        {
            ObservableCollection<ILineasParametros> lista = new ObservableCollection<ILineasParametros>();

            PersistenceManager.SelectByProperty<LineasRevisionOferta>("IdRevisionOferta", idRevision)
                .ForEach(l => lista.Add(
                    new ILineasParametros
                    {
                        IdParametro = l.IdParametro,
                        Cantidad = l.Cantidad,
                    })
                );
            return lista;
        }


        public static bool ValorUnico<T>(string nombrePropiedad, T valor) where T : PersistenceData, IModelo
        {
            var valorPropiedad = valor.GetType().GetProperty(nombrePropiedad).GetValue(valor);

            return PersistenceManager.SelectByProperty<T>(nombrePropiedad, valorPropiedad)
                .Where(p => p.Id != valor.Id)
                .Count() == 0;
        }

        public static bool ValorUnicoOVacio<T>(string nombrePropiedad, T valor) where T : PersistenceData, IModelo
        {
            var valorPropiedad = valor.GetType().GetProperty(nombrePropiedad).GetValue(valor);

            if (valorPropiedad == null || valorPropiedad.Equals(""))
                return true;

            return PersistenceManager.SelectByProperty<T>(nombrePropiedad, valorPropiedad)
                .Where(p => p.Id != valor.Id)
                .Count() == 0;
        }

        public static void VisualizeWindow(MahApps.Metro.Controls.MetroWindow window)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = window.Width;
            double windowHeight = window.Height;
            if (screenHeight < windowHeight || screenWidth < windowWidth)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

    }
}
