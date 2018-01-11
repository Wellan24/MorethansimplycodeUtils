using LAE.Modelo;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Analisis
{
    /// <summary>
    /// Lógica de interacción para PageHumedad.xaml
    /// </summary>
    public partial class PageHumedad : UserControl
    {

        private MedicionPNT[] mediciones;
        public MedicionPNT[] Mediciones
        {
            get { return mediciones; }
            set
            {
                mediciones = value;
                IdMuestra = mediciones[0].IdMuestra;
                IdTecnicoRecepcion = mediciones[0].IdTecnico;
                CargarMedicion();
            }
        }

        public int IdMuestra;
        public int IdTecnicoRecepcion;

        public PageHumedad()
        {
            InitializeComponent();
        }

        private void CargarMedicion()
        {
            foreach (MedicionPNT med in Mediciones)
            {
                AddControl(med);
            }
        }

        private void AddControl(MedicionPNT med)
        {
            MedicionHumedad medicion = new MedicionHumedad() { Medicion = med };
            medicion.DeleteControl = BorrarMedicion;
            listaMediciones.Children.Add(medicion);
        }

        private void NuevaMedicion_Click(object sender, RoutedEventArgs e)
        {
            AddControl(FactoriaMedicionPNT.GetDefault(IdTecnicoRecepcion, IdMuestra, true));
        }

        private void BorrarMedicion(MedicionHumedad control)
        {
            if (ValidarBorrado(control))
                listaMediciones.Children.Remove(control);
            else
                MessageBox.Show("Humedad no se puede borrar esta siendo usada en el cálculo de otro parámetro");
        }

        private bool ValidarBorrado(MedicionHumedad control)
        {
            /* En principio no usare la humedad del CCI, para otros cálculos, si la usara también debería validarlo */
            int nHumedad = control.Prueba.Humedad.Id;
            return !PersistenceManager.SelectByProperty<Densidad>("IdHumedad", nHumedad).Any();
        }
    }
}
