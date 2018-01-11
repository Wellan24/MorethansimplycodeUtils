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
    /// Lógica de interacción para Particulas.xaml
    /// </summary>
    public partial class Soportes : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
           DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(Soportes), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private Soporte soporte;
        public Soporte Soporte
        {
            get { return soporte; }
            set
            {
                soporte = value;
                GenerarSoporte();
            }
        }

        public Soportes()
        {
            InitializeComponent();
            GenerarSoporte();
        }

        private void GenerarSoporte()
        {
            UCMedicionAnterior.GenerarPrueba();
            UCMedicionPosterior.GenerarPrueba2();
        }


    }
}
