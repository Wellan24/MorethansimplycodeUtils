using Cartif.Expectation;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Comun.Clases;
using LAE.Modelo;
using MahApps.Metro.Controls;
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
    /// Lógica de interacción para NuevoContacto.xaml
    /// </summary>
    public partial class NuevoContacto : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(NuevoContacto), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public Contacto Contacto { get; set; }

        public NuevoContacto()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            Contacto = new Contacto();
        }

        public NuevoContacto(Tecnico[] tecnicos)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE.Comun;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            Contacto = new Contacto();
            GenerarPanel(tecnicos);
        }

        private void GenerarPanel(Tecnico[] tecnicos)
        {
            panelContactos.Build(Contacto,
                new TypePanelSettings<Contacto>
                {
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                                        .SetLabel("* Nombre"),
                        ["Apellidos"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Email"] = PropertyControlSettingsEnum.TextBoxIsValidEmail
                                        .SetLabel("* Email"),
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(FactoriaClientes.RecuperarClientes())
                                .SetLabel("* Cliente"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(tecnicos)
                                .SetLabel("* Técnico")
                    },
                    PanelValidation = Expectation<Contacto>
                        .ShouldBe().AddCriteria(c => Util.ValorUnico<Contacto>("Email", c))
                });
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (panelContactos.GetValidatedInnerValue<Contacto>() != default(Contacto))
            {
                Contacto = panelContactos.InnerValue as Contacto;
                int idContacto = Contacto.Insert();
                Contacto.Id = idContacto;
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
