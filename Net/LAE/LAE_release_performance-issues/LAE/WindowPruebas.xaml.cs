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

namespace LAE
{
    /// <summary>
    /// Lógica de interacción para WindowPruebas.xaml
    /// </summary>
    public partial class WindowPruebas : Window
    {
        public WindowPruebas()
        {
            InitializeComponent();
        }

        private void mostrarCRM_clicked(object sender, MouseButtonEventArgs e)
        {
            if (crm1.IsVisible)
            {
                crm1.Visibility = Visibility.Collapsed;
                crm2.Visibility = Visibility.Collapsed;
                crm3.Visibility = Visibility.Collapsed;
            }
            else
            {
                crm1.Visibility = Visibility.Visible;
                crm2.Visibility = Visibility.Visible;
                crm3.Visibility = Visibility.Visible;
            }
        }

    }
}
