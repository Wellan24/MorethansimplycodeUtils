using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Comun.Clases;
using LAE.Comun.Modelo;
using LAE.Modelo;
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

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para NuevoTipoMuestra.xaml
    /// </summary>
    public partial class NuevoTipoMuestra : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(NuevoTipoMuestra), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }


        public TipoMuestra TipoMuestra { get; set; }

        public NuevoTipoMuestra()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            TipoMuestra = new TipoMuestra();
            GenerarPanel();
        }

        private void GenerarPanel()
        {
            panelTiposMuestra.Build(TipoMuestra,
                 new TypePanelSettings<TipoMuestra>
                 {
                     Fields = new FieldSettings
                     {
                         ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty,
                     },
                     ColumnWidths=new int[] {1}
                 });
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (panelTiposMuestra.GetValidatedInnerValue<TipoMuestra>() != default(TipoMuestra))
            {
                TipoMuestra = panelTiposMuestra.InnerValue as TipoMuestra;
                int idTipoMuestra = TipoMuestra.Insert();
                TipoMuestra.Id = idTipoMuestra;
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
