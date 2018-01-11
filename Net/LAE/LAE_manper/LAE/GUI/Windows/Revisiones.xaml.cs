using Cartif.Logs;
using Dapper;
using GUI.Controls;
using LAE.Comun.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LAE.Comun.Modelo;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para Revisiones.xaml
    /// </summary>
    public partial class Revisiones : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(Revisiones), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public Revisiones()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
        }

        public Revisiones(Tecnico[] tecnicos, RevisionOferta revision)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            UCRevision.Tecnicos = tecnicos;
            UCRevision.Revision = revision;
            if (revision.Enviada || revision.Aceptada)
            {
                LinkPeticionParent.Visibility = Visibility.Collapsed;
                bGuardarRevision.Visibility = Visibility.Collapsed;
                bCancelarRevision.Content = "Salir";
            }
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (UCRevision.ValidarRevision())
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        RevisionOferta rev = UCRevision.Revision;
                        int idRevision = GuardarRevision(conn, rev);
                        rev.Id = idRevision;
                        GuardarTipoMuestra(conn, idRevision);
                        GuardarPuntosControl(conn, idRevision);

                        trans.Commit();
                        DialogResult = true;
                        MessageBox.Show("Datos guardados con éxito");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar la revisión de la oferta", ex);
                        MessageBox.Show("Se ha producido un error al intentar guardar los datos. Por favor, recargue la página o informa a soporte.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }

        }

        private int GuardarRevision(NpgsqlConnection conn, RevisionOferta rev)
        {
            if (rev.Id == 0)
            {
                StringBuilder consulta = new StringBuilder("SELECT insertarrevision(@Observaciones, @FechaEmision, @Importe, @RequiereTomaMuestra, @LugarMuestra, @NumPuntosMuestreo, @TrabajoPuntual, @Frecuencia, @PlazoRealizacion, @IdOferta, @IdTecnico)");
                int idRevision = conn.Query<int>(consulta.ToString(), rev).FirstOrDefault();
                if (idRevision == 0)
                    throw new PersistenceDataException("Error al guardar la revisión");
                return idRevision;
            }

            if (!rev.Update(conn))
                throw new PersistenceDataException("Error al actualizar la revisión");
            return rev.Id;
        }

        private void GuardarTipoMuestra(NpgsqlConnection conn, int idRevision)
        {
            List<TipoMuestraRevision> lineas = PersistenceManager.SelectByProperty<TipoMuestraRevision>("IdRevision", idRevision).ToList();

            foreach (ITipoMuestra item in UCRevision.LineasTipoMuestra)
            {
                TipoMuestraRevision tmp = new TipoMuestraRevision();
                tmp.IdTipoMuestra = item.IdTipoMuestra;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    tmp.IdRevision = idRevision;
                    int n = tmp.Insert(conn);
                    if (n == 0)
                        throw new PersistenceDataException("Error al guardar los tipos de muestra");
                }
                else
                {
                    /* actualizo existentes */
                    tmp.Id = item.Id;
                    tmp.IdRevision = item.IdRelacion;

                    /* NO SE ACTUALIZA PORQUE NO PUEDO MODIFICAR DATOS, SOLO AÑADIR O BORRAR
                    if (!tmp.Update(conn))
                        throw new PersistenceDataException("Error al actualizar los tipos de muestra");
                    */

                    lineas.Remove(tmp);
                }
            }
            foreach (TipoMuestraRevision item in lineas)
            {
                /* elimino borrados */
                if (!item.Delete(conn))
                    throw new PersistenceDataException("Error al borrar los tipos de muestra");
            }
        }

        private void GuardarPuntosControl(NpgsqlConnection conn, int idRevision)
        {
            List<PuntocontrolRevision> puntosControlBBDD = PersistenceManager.SelectByProperty<PuntocontrolRevision>("IdRevision", idRevision).ToList();
            foreach (ControlLineaParametro clp in UCRevision.LineasPuntosControl)
            {
                IPuntoControl ipc = clp.PuntoControl;
                PuntocontrolRevision pcr = new PuntocontrolRevision
                {
                    Nombre = ipc.Nombre,
                    Observaciones = ipc.Observaciones,
                    Importe = ipc.Importe,
                    IdRevision = idRevision
                };
                if (ipc.Id == 0)
                {
                    /* inserto nuevo*/
                    int idPuntoControl = pcr.Insert(conn);
                    if (idPuntoControl == 0)
                        throw new PersistenceDataException("Error al insertar puntos de control");
                    GuardarParametros(conn, ipc, idPuntoControl);
                }
                else
                {
                    /* actualizo */
                    ActualizarParametros(conn, ipc);
                    pcr.Id = ipc.Id;
                    if (!pcr.Update(conn))
                        throw new PersistenceDataException("Error al actualizar puntos de control");
                    puntosControlBBDD.Remove(pcr);
                }
            }

            foreach (PuntocontrolRevision pcp in puntosControlBBDD)
            {
                /* elimino borrados */
                EliminarParametros(conn, pcp);
                if (!pcp.Delete(conn))
                    throw new PersistenceDataException("Error al borrar puntos de control");
            }
        }

        private void ActualizarParametros(NpgsqlConnection conn, IPuntoControl ipc)
        {
            List<LineaRevisionOferta> lineas = PersistenceManager.SelectByProperty<LineaRevisionOferta>("IdPControlRevisionOferta", ipc.Id).ToList();

            foreach (ILineasParametros ilp in ipc.Lineas)
            {

                LineaRevisionOferta lp = new LineaRevisionOferta
                {
                    Cantidad = ilp.Cantidad,
                    IdParametro = ilp.IdParametro,
                    IdPControlRevisionOferta = ipc.Id
                };

                if (ilp.Id == 0)
                {
                    int n = lp.Insert(conn);
                    if (n == 0)
                        throw new PersistenceDataException("Error al guardar los parámetros");
                }
                else {
                    /* no hay update */
                    lp.Id = ilp.Id;
                    lineas.Remove(lp);
                }
            }

            foreach (LineaRevisionOferta lp in lineas)
            {
                if (!lp.Delete(conn))
                    throw new PersistenceDataException("Error al borrar los parámetros");
            }
        }

        private void GuardarParametros(NpgsqlConnection conn, IPuntoControl ipc, int idPuntoControl)
        {
            foreach (ILineasParametros ilp in ipc.Lineas)
            {
                LineaRevisionOferta lp = new LineaRevisionOferta
                {
                    Cantidad = ilp.Cantidad,
                    IdParametro = ilp.IdParametro,
                    IdPControlRevisionOferta = idPuntoControl
                };
                int n = lp.Insert(conn);
                if (n == 0)
                    throw new PersistenceDataException("Error al guardar parámetros");
            }
        }

        private void EliminarParametros(NpgsqlConnection conn, PuntocontrolRevision pcp)
        {
            List<LineaRevisionOferta> lineas = PersistenceManager.SelectByProperty<LineaRevisionOferta>("IdPControlRevisionOferta", pcp.Id).ToList();
            foreach (LineaRevisionOferta lp in lineas)
            {
                if (!lp.Delete(conn))
                    throw new PersistenceDataException("Error al borrar parámetros");
            }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void ComboCargar_Click(object sender, RoutedEventArgs e)
        {
            ComboCargarRevision combo = new ComboCargarRevision();
            combo.Lista = LAE.Clases.Util.ComboBoxCodigoOfertas();
            combo.Owner = Window.GetWindow(this);
            combo.ShowDialog();

            if (combo.DialogResult ?? false)
            {
                RevisionOferta r = PersistenceManager.SelectByID<RevisionOferta>(combo.idSeleccionado);
                r.Id = 0;
                r.IdOferta = UCRevision.Revision.IdOferta;
                r.Num = UCRevision.Revision.Num;
                r.FechaEmision = UCRevision.Revision.FechaEmision;

                UCRevision.CargarNuevaRevision(r);
                UCRevision.CargarTipoMuestra(LAE.Clases.Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                UCRevision.CargarParametros(combo.idSeleccionado);

                MessageBox.Show("Datos cargados con éxito");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
