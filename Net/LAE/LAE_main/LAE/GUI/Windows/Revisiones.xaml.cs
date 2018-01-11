using Cartif.Logs;
using Dapper;
using LAE.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Npgsql;
using Persistence;
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
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
        }

        public Revisiones(Tecnico[] tecnicos, RevisionOferta revision)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            UCRevision.Tecnicos = tecnicos;
            UCRevision.Revision = revision;
            if (revision.Enviada)
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
                RevisionOferta rev = UCRevision.Revision;
                GuardarRevision(rev);
                if (rev.Id != 0)
                {
                    GuardarTipoMuestra(rev);
                    GuardarParametros(rev);
                    MessageBox.Show("Datos guardados con éxito");
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Se ha producido un error al guardar la revisión. Por favor, vuelve a intentarlo o informa a soporte.");
                    DialogResult = false;
                }

                this.Close();
            }
        }

        private void GuardarRevision(RevisionOferta rev)
        {
            if (rev.Id == 0)
            {
                int idRevision;
                StringBuilder consulta = new StringBuilder("SELECT insertarrevision(@Observaciones, @FechaEmision, @Importe, @RequiereTomaMuestra, @LugarMuestra, @NumPuntosMuestreo, @TrabajoPuntual, @Frecuencia, @PlazoRealizacion, @IdOferta, @IdTecnico)");
                try
                {
                    using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                        idRevision = conn.Query<int>(consulta.ToString(), rev).FirstOrDefault();
                    rev.Id = idRevision;
                }
                catch (Exception ex)
                {
                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                    MessageBox.Show("Se ha producido un error al guardar la revisión. Por favor, vuelve a intentarlo o informa a soporte.");
                }
            }
            else
                rev.Update();
        }

        private void GuardarTipoMuestra(RevisionOferta rev)
        {
            List<TipoMuestraRevision> lineas = PersistenceManager.SelectByProperty<TipoMuestraRevision>("IdRevision", rev.Id).ToList();

            foreach (ITipoMuestra item in UCRevision.lineasTipoMuestra)
            {
                TipoMuestraRevision tmr = new TipoMuestraRevision();
                tmr.IdTipoMuestra = item.IdTipoMuestra;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    tmr.IdRevision = rev.Id;
                    tmr.Insert();
                }
                else
                {
                    /* actualizo existentes */
                    tmr.Id = item.Id;
                    tmr.IdRevision = item.IdRelacion;
                    tmr.Update();

                    lineas.Remove(tmr);
                }
            }
            foreach (TipoMuestraRevision item in lineas)
            {
                /* elimino borrados */
                item.Delete();
            }
        }

        private void GuardarParametros(RevisionOferta rev)
        {
            List<LineasRevisionOferta> lineas = PersistenceManager.SelectByProperty<LineasRevisionOferta>("IdRevisionOferta", rev.Id).ToList();

            foreach (ILineasParametros item in UCRevision.lineasParametros)
            {
                LineasRevisionOferta lr = new LineasRevisionOferta();
                lr.Cantidad = item.Cantidad;
                lr.IdParametro = item.IdParametro;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    lr.IdRevisionOferta = rev.Id;
                    lr.Insert();
                }
                else
                {
                    /* actualizo existentes */
                    lr.IdRevisionOferta = rev.Id;
                    lr.Id = item.Id;
                    lr.Update();

                    lineas.Remove(lr);
                }
            }
            foreach (LineasRevisionOferta item in lineas)
            {
                /* elimino borrados */
                item.Delete();
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
            combo.Lista = Util.ComboBoxCodigoOfertas();
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
                UCRevision.CargarTipoMuestra(Util.GetTiposMuestraFromRevision(combo.idSeleccionado));
                UCRevision.CargarParametro(Util.GetParametrosFromRevision(combo.idSeleccionado));

                MessageBox.Show("Datos cargados con éxito");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
