using LAE.Modelo;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlLineaPuntoMuestreo.xaml
    /// </summary>
    public partial class ControlLineaPuntoMuestreo : UserControl
    {
        private ConexionFocoAtm conexion;
        public ConexionFocoAtm Conexion
        {
            get { return conexion; }
            set
            {
                conexion = value;
                GenerarDatosLinea();
            }
        }

        public ControlLineaPuntoMuestreo()
        {
            InitializeComponent();
        }

        private void GenerarDatosLinea()
        {
            foreach (PuntoConexFocoAtmosfera punto in Conexion.PuntosMuestreo)
            {
                ControlPuntoMuestreoPM cp = new ControlPuntoMuestreoPM() { PuntoConexion = punto };
                numConexion.Content = String.Format("D{0}", Conexion.NumConexion);
                lineaPuntos.Children.Add(cp);
            }
        }
    }
}
