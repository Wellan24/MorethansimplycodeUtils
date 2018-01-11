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

namespace GUI.Analisis
{
    /// <summary>
    /// Lógica de interacción para PageHumedad3Viejo.xaml
    /// </summary>
    public partial class PageHumedad3Viejo : UserControl
    {
        private Humedad3[] humedad;
        public Humedad3[] Humedad
        {
            get { return humedad; }
            set
            {
                humedad = value;
            }
        }

        private MedicionPNT[] mediciones;
        public MedicionPNT[] Mediciones
        {
            get { return mediciones; }
            set
            {
                mediciones = value;
                IdMuestra = mediciones[0].IdMuestra;
                IdTecnicoRecepcion = mediciones[0].IdTecnico;
                CargarHumedad();
            }
        }

        public int IdMuestra;
        public int IdTecnicoRecepcion;

        public PageHumedad3Viejo()
        {
            InitializeComponent();
        }

        private void CargarHumedad()
        {
            foreach (MedicionPNT med in Mediciones)
            {
                ControlHumedad3Viejo medicion = new ControlHumedad3Viejo() { Medicion = med };
                medicion.DeleteControl = BorrarMedicion;
                listaMediciones.Children.Add(medicion);
            }
        }

        private void NuevaMedicion_Click(object sender, RoutedEventArgs e)
        {
            ControlHumedad3Viejo medicion = new ControlHumedad3Viejo() { Medicion = FactoriaMedicionPNT.GetDefault(IdTecnicoRecepcion, IdMuestra) };
            medicion.DeleteControl = BorrarMedicion;
            listaMediciones.Children.Add(medicion);
            
        }

        private void BorrarMedicion(ControlHumedad3Viejo control)
        {
            listaMediciones.Children.Remove(control);
        }
    }
}
