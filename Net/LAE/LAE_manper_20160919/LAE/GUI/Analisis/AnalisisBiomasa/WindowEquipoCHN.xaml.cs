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

namespace GUI.Analisis
{
    /// <summary>
    /// Lógica de interacción para WindowEquipoCHN.xaml
    /// </summary>
    public partial class WindowEquipoCHN : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
               DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(WindowEquipoCHN), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private EnsayoPNT ensayo;

        public EnsayoPNT Ensayo
        {
            get { return ensayo; }
            set
            {
                ensayo = value;
                CHNcontrol = FactoriaChnControl.GetControles(Ensayo.Id);
            }
        }

        private CHNcontrol[] chncontrol;

        public CHNcontrol[] CHNcontrol
        {
            get { return chncontrol; }
            set
            {
                chncontrol = value;
                CargarCCI();
            }
        }

        public WindowEquipoCHN()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            CargarDatos();
        }

        private void CargarDatos()
        {
            //TODO temporal-borrar
            Ensayo = PersistenceManager.SelectByID<EnsayoPNT>(1);
        }

        private void CargarCCI()
        {
            foreach (CHNcontrol c in CHNcontrol)
            {
                AddControl(c);
            }
        }

        private void AddControl(CHNcontrol c)
        {
            ControlCHNcci control = new ControlCHNcci() { CHNcontrol = c };
            control.DeleteControl = BorrarControl;
            listaCCI.Children.Add(control);
        }

        private void NuevoCCI_Click(object sender, RoutedEventArgs e)
        {
            AddControl(FactoriaChnControl.GetDefault(Ensayo.Id));
        }

        private void BorrarControl(ControlCHNcci control)
        {
            listaCCI.Children.Remove(control);
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
