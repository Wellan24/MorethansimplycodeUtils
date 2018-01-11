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
    /// Lógica de interacción para ComboCargarRevision.xaml
    /// </summary>
    public partial class ComboCargarRevision : MetroWindow
    {

        public static readonly DependencyProperty IconTitleProperty =
            DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(ComboCargarRevision), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        public int idSeleccionado { get; set; }

        public List<KeyValuePair<int, String>> Lista
        {
            set
            {
                if (value != null)
                    comboRevision.ItemsSource = value;
            }
        }

        public ComboCargarRevision()
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void bCargar_Click(object sender, RoutedEventArgs e)
        {
            if (comboRevision.SelectedItem != null)
            {
                idSeleccionado = ((KeyValuePair<int, String>)comboRevision.SelectedItem).Key;
                DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("Elige un valor que este en la lista");
        }
    }
}
