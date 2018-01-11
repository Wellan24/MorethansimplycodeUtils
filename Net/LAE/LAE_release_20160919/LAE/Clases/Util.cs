using Cartif.Extensions;
using Cartif.Logs;
using Dapper;
using GUI.Controls;
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
using System.Windows.Media;

namespace LAE.Clases
{
    static class Util
    {
        public static List<KeyValuePair<int, string>> ComboBoxCodigoOfertasTrabajos(Oferta o)
        {

            StringBuilder consulta = new StringBuilder(@"SELECT id_revisionoferta as Key, right(anno_oferta::text, -2) || '-' || numcodigo_oferta || '/' || num_revisionoferta as Value
                                    from ofertas
                                    inner join revisiones_oferta on id_oferta = idoferta_revisionoferta
                                    where anulada_oferta = false
                                    and id_oferta= :IdOferta
                                    and aceptada_revisionoferta=true
                                    order by num_revisionoferta desc");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<KeyValuePair<int, String>>(consulta.ToString(), new { IdOferta = o?.Id }).ToList();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al obtener los códigos de las ofertas");
                return null;
            }
        }

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

        public static List<ControlLineaParametro> GetParametrosFromRevision(int idRevision, bool withId = false)
        {
            RevisionOferta rev = PersistenceManager.SelectByProperty<RevisionOferta>("Id", idRevision).FirstOrDefault();
            ControlLineaParametro c;
            List<ControlLineaParametro> lineasPuntosControl = new List<ControlLineaParametro>();

            rev.LoadPuntosControl();
            rev.PuntosControl.ForEach(pc =>
            {
                pc.LoadLineasRevision();

                c = new ControlLineaParametro { PuntoControl = FactoriaIPuntoControl.GetIPuntoControl(pc, withId) };
                lineasPuntosControl.Add(c);
            });

            return lineasPuntosControl;
        }

        public static List<ControlLineaParametro> GetParametrosFromPeticion(int idPeticion)
        {
            Peticion pet = PersistenceManager.SelectByProperty<Peticion>("Id", idPeticion).FirstOrDefault();
            ControlLineaParametro c;
            ObservableCollection<ILineasParametros> lineas;
            List<ControlLineaParametro> lineasPuntosControl = new List<ControlLineaParametro>();

            pet.LoadPuntosControl();
            pet.PuntosControl.ForEach(pc =>
            {
                lineas = new ObservableCollection<ILineasParametros>();

                pc.LoadLineasPeticion();

                c = new ControlLineaParametro { PuntoControl = FactoriaIPuntoControl.GetIPuntoControl(pc) };
                lineasPuntosControl.Add(c);
            });

            return lineasPuntosControl;
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

        /// <summary> Show a message, if accept close the window </summary>
        /// <remarks> manper </remarks>
        /// <param name="ventana">Window to close</param>
        public static void MensajeCancel(this Window ventana)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Deseas salir sin guardar?.", "Salir", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ventana.DialogResult = false;
                ventana.Close();
            }
        }

        public static void Aceptacion(this System.Windows.Controls.Label label, bool aceptado)
        {
            if (aceptado)
            {
                label.Content = "OK";
                label.Background = new SolidColorBrush(Color.FromRgb(26, 148, 49));
            }
            else
            {
                label.Content = "Rechazo";
                label.Background = new SolidColorBrush(Color.FromRgb(193, 46, 34));
            }
            label.Visibility = Visibility.Visible;
        }

    }
}
