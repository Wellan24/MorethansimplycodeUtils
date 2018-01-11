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
    /// Lógica de interacción para ControlLineaCabeceraPuntoMuestreo.xaml
    /// </summary>
    public partial class ControlLineaCabeceraPuntoMuestreo : UserControl
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

        public ControlLineaCabeceraPuntoMuestreo()
        {
            InitializeComponent();
        }

        private void GenerarDatosLinea()
        {
            foreach (PuntoConexFocoAtmosfera punto in Conexion.PuntosMuestreo)
            {
                ControlCabeceraPuntoMuestreo cp = new ControlCabeceraPuntoMuestreo() { PuntoConexion = punto };
                lineaPuntos.Children.Add(cp);
            }
        }
    }
}
