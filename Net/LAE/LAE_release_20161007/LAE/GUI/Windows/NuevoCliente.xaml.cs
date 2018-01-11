using Cartif.Expectation;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
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
    /// Lógica de interacción para NuevoCliente.xaml
    /// </summary>
    public partial class NuevoCliente : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(NuevoCliente), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public Cliente Cliente { get; set; }

        public NuevoCliente()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Cliente = new Cliente() { Pais = "ESPAÑA" };
            GenerarPanel();
        }

        private void GenerarPanel()
        {
            panelClientes.Build(Cliente, new TypePanelSettings<Cliente>
            {
                Fields = new FieldSettings
                {
                    ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                .SetLabel("* Nombre"),
                    ["Direccion"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Fax"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Email"] = PropertyControlSettingsEnum.TextBoxIsValidEmailOrEmpty,
                    ["Web"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Cif"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["CodigoPostal"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Localidad"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Provincia"] = PropertyControlSettingsEnum.TextBoxDefault,
                    ["Pais"] = PropertyControlSettingsEnum.TextBoxDefault,
                },
                PanelValidation = Expectation<Cliente>
                        .ShouldBe().AddCriteria(c => Util.ValorUnicoOVacio("Cif", c))
                        .AddCriteria(c => Util.ValorUnicoOVacio("Email", c))
            });
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (panelClientes.GetValidatedInnerValue<Cliente>() != default(Cliente))
            {
                Cliente = panelClientes.InnerValue as Cliente;
                int idCliente = Cliente.Insert();
                Cliente.Id = idCliente;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Util.VisualizeWindow(this);
        }
    }
}
