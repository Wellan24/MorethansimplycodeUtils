using MahApps.Metro.Controls;
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
using LAE.Modelo;
using LAE.Comun.Clases;
using LAE.Comun.Persistence;
using GenericForms.Settings;
using GenericForms.Abstract;
using Cartif.Util;
using GenericForms.Implemented;
using Npgsql;
using Dapper;
using Cartif.Logs;
using Microsoft.Win32;
using System.IO;
using LAE.Comun.Modelo;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para Trabajo.xaml
    /// </summary>
    public partial class Trabajos : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
           DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(Trabajos), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private Tecnico[] tecnicos { get; set; }
        private RevisionOferta revision;
        private Oferta oferta { get; set; }

        private Trabajo trabajo;
        public Trabajo Trabajo
        {
            get { return trabajo; }
            set
            {
                trabajo = value;
                if (trabajo.Id == 0)
                {
                    LoadDefaultData();
                }

                GenerarPanelTrabajo();
                MostrarLinkDocumento();
            }
        }

        private void LoadDefaultData()
        {

            trabajo.IdContacto = oferta.IdContacto;
            trabajo.IdTecnico = revision.IdTecnico;
            trabajo.FechaFirmaCliente = DateTime.Now;
            trabajo.FechaFirmaLae = DateTime.Now;


            trabajo.IdOferta = oferta.Id;
        }

        public Trabajos(Tecnico[] tecnicos, RevisionOferta revision, Oferta oferta)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            this.tecnicos = tecnicos;
            this.revision = revision;
            this.oferta = oferta;
        }

        private void GenerarPanelTrabajo()
        {
            panelTrabajo.Build(Trabajo, new TypePanelSettings<Trabajo>
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
                    return conn.Query<Contacto>(consulta.ToString(), new { IdContacto = trabajo.IdContacto }).ToArray();
                }
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Se ha producido un error al recuperar los contactos. Por favor, informa a soporte.");
                return new Contacto[0];
            }
        }

        private void MostrarLinkDocumento()
        {
            if (trabajo.DocumentoFirma != null)
                LinkDocumentoParent.Visibility = Visibility.Visible;
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Deseas salir sin guardar?.", "Salir", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DialogResult = false;
                this.Close();
            }
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea borrar la solicitud de ensayo?", "Borrar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (trabajo.Delete())
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al borrar la solicitud de ensayo");
                    MessageBox.Show("Se ha producido un error al borrar la solicitud de ensayo. Por favor, informa a soporte.");
                }
            }
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (trabajo.Id == 0)
            {
                String consulta = "SELECT insertartrabajo (@DocumentoFirma, @NombreDocumento, @Observaciones, @FirmadoCliente, @FirmadoLae, @FechaFirmaCliente, @FechaFirmaLae, @IdContacto, @IdTecnico, @IdOferta)";
                int idTrabajo = 0;

                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    idTrabajo = conn.Query<int>(consulta, trabajo).FirstOrDefault();

                if (idTrabajo != 0)
                {
                    MessageBox.Show("Solicitud guardada con éxito");
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar la solicitud de ensayo");
                    MessageBox.Show("Se ha producido un error al guardar la solicitud de ensayo. Por favor, informa a soporte.");
                }
            }
            else
            {
                if (trabajo.Update())
                {
                    MessageBox.Show("Solicitud de ensayo actualizada con éxito");
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al actualizar la solicitud de ensayo");
                    MessageBox.Show("Se ha producido un error al actualizar la solicitud de ensayo. Por favor, informa a soporte.");
                }
            }

        }

        private void bUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.DefaultExt = ".docx";
            //dlg.Filter = "Word documents (.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                trabajo.DocumentoFirma = File.ReadAllBytes(dlg.FileName);
                trabajo.NombreDocumento = dlg.SafeFileName;
                LinkDocumentoParent.Visibility = Visibility.Visible;
            }
        }

        private void bDocDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LinkDocumentoParent.Visibility == Visibility.Visible)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que desea borrar el fichero de aceptación de la solicitud de ensayo?. Si al finalizar la edición guarda los cambios el archivo será borrado", "Rechazar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    trabajo.DocumentoFirma = null;
                    trabajo.NombreDocumento = null;
                    LinkDocumentoParent.Visibility = Visibility.Collapsed;
                }
            }
            else
                MessageBox.Show("No existe fichero de aceptación de la solicitud de ensayo");
        }

        private void VerDocumento_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //File.WriteAllBytes(dialog.SelectedPath + "/" + oferta.Codigo + "-SE-" + Trabajo.NumCodigo + ".docx", trabajo.DocumentoFirma);
                //File.ReadAllBytes(dialog.SelectedPath + "/" + oferta.Codigo + "-SE-" + Trabajo.NumCodigo + ".docx");
                File.WriteAllBytes(dialog.SelectedPath + "/" + Trabajo.NombreDocumento, trabajo.DocumentoFirma);
                File.ReadAllBytes(dialog.SelectedPath + "/" + Trabajo.NombreDocumento);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
