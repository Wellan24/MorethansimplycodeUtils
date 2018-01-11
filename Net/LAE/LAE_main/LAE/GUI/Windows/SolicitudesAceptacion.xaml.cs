using Cartif.Logs;
using Cartif.Util;
using Dapper;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Npgsql;
using Persistence;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para SolicitudAceptacion.xaml
    /// </summary>
    public partial class SolicitudesAceptacion : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(SolicitudesAceptacion), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public Tecnico[] tecnicos { get; set; }
        public SolicitudAceptacion solicitud { get; set; }
        public RevisionOferta revision { get; set; }

        public SolicitudesAceptacion()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
        }

        public SolicitudesAceptacion(Tecnico[] tecnicos, RevisionOferta revision)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            this.tecnicos = tecnicos;
            this.revision = revision;

            solicitud = PersistenceManager.SelectByProperty<SolicitudAceptacion>("IdRevisionOferta", revision.Id).FirstOrDefault();
            if (solicitud == null)
                solicitud = CargarSolicitud(revision);
            GenerarPanelSolicitud();
            MostrarLinkDocumento();
        }

        private SolicitudAceptacion CargarSolicitud(RevisionOferta rev)
        {
            Oferta o = PersistenceManager.SelectByProperty<Oferta>("Id", rev.IdOferta).FirstOrDefault();
            SolicitudAceptacion s = new SolicitudAceptacion
            {
                IdContacto = o.IdContacto,
                IdTecnico = rev.IdTecnico,
                FechaFirmaCliente = DateTime.Now,
                FechaFirmaLae = DateTime.Now
            };
            return s;
        }

        private void GenerarPanelSolicitud()
        {
            panelSolicitud.Build(solicitud, new TypePanelSettings<SolicitudAceptacion>
            {
                Fields = new FieldSettings
                {
                    ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                    .SetInnerValues(RecuperarContactos())
                                    .SetLabel("* Contacto"),
                    ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                    .SetInnerValues(tecnicos)
                                    .SetLabel("* Técnico"),
                    ["FirmadoCliente"] = new PropertyControlSettings
                    {
                        InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                        },
                        Type = typeof(PropertyControlComboBox)
                    }
                        .SetLabel("Firmado por Cliente"),
                    ["FechaFirmaCliente"] = PropertyControlSettingsEnum.DateTimeDefault
                        .SetLabel("Fecha firma Cliente"),
                    ["FirmadoLae"] = new PropertyControlSettings
                    {
                        InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                        },
                        Type = typeof(PropertyControlComboBox)
                    }
                        .SetLabel("Firmado por el LAE"),
                    ["FechaFirmaLae"] = PropertyControlSettingsEnum.DateTimeDefault
                        .SetLabel("Fecha firma LAE"),
                    ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                        .SetHeightMultiline(90)
                        .SetColumnSpan(2),

                },
                IsUpdating = true,
                ColumnWidths = new int[] { 1, 1 }
            });

        }

        private void MostrarLinkDocumento()
        {
            if (solicitud.DocumentoFirma != null)
                LinkDocumentoParent.Visibility = Visibility.Visible;
        }

        private Contacto[] RecuperarContactos()
        {
            StringBuilder consulta = new StringBuilder(@"select c1.id_contacto Id, c1.nombre_contacto Nombre, c1.apellidos_contacto Apellidos, c1.telefono_contacto Telefono, c1.email_contacto Email, c1.idcliente_contacto IdCliente, c1.idtecnico_contacto IdTecnico
                                                            from contactos c
                                                            inner join clientes on c.idcliente_contacto = id_cliente
                                                            inner join contactos c1 on id_cliente = c1.idcliente_contacto
                                                            where c.id_contacto = :IdContacto ");
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    return conn.Query<Contacto>(consulta.ToString(), new { IdContacto = solicitud.IdContacto }).ToArray();
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al recuperar los contactos. Por favor, informa a soporte.");
                return new Contacto[0];
            }

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Deseas salir sin guardar?.", "Salirr", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DialogResult = false;
                this.Close();
            }
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea rechazar la solicitud?. Se asignará la solicitud como no aceptada y no se guardaran los últimos cambios", "Rechazar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                revision.Aceptada = false;
                revision.Update(columnsToUpdate: "Aceptada");
                DialogResult = true;
                this.Close();
            }
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            revision.Aceptada = true;
            revision.Update(columnsToUpdate: "Aceptada");

            if (solicitud.Id == 0)
            {
                solicitud.IdRevisionOferta = revision.Id;
                solicitud.Insert();
                MessageBox.Show("Solicitud guardada con éxito");
            }
            else {
                solicitud.Update();
                MessageBox.Show("Solicitud actualizada con éxito");
            }

            DialogResult = true;
            this.Close();
        }

        private void bUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".docx";
            dlg.Filter = "Word documents (.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                solicitud.DocumentoFirma = File.ReadAllBytes(dlg.FileName);
                LinkDocumentoParent.Visibility = Visibility.Visible;
            }
        }

        private void VerDocumento_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //TODO cambiar nombre al archivo recuperado
                File.WriteAllBytes(dialog.SelectedPath + "/" + "SE-LAE-prueba.docx", solicitud.DocumentoFirma);
            }
        }

        private void bDocDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LinkDocumentoParent.Visibility == Visibility.Visible)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que desea borrar el fichero de solicitud de ensayo?. Si al finalizar la edición guarda los cambios el archivo será borrado", "Rechazar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    solicitud.DocumentoFirma = null;
                    LinkDocumentoParent.Visibility = Visibility.Collapsed;
                }
            }
            else
                MessageBox.Show("No existe fichero de solicitud de ensayo");
        }
    }
}
