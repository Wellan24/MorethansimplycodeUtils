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
    /// Lógica de interacción para PageHumedadTotalViejo2.xaml
    /// </summary>
    public partial class PageHumedadTotalViejo2 : UserControl
    {
        private HumedadTotal[] humedad;
        public HumedadTotal[] Humedad
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

        public PageHumedadTotalViejo2()
        {
            InitializeComponent();
        }

        private void CargarHumedad()
        {
            foreach (MedicionPNT med in Mediciones)
            {
                AddControl(med);
                /*Add CCI*/
                MedicionPNT medCCI = FactoriaMedicionPNTcci.GetMedicion(med.Id);
                if (medCCI != null)
                    AddControl(medCCI, true);
            }
        }

        private void AddControl(MedicionPNT med, bool CCI=false)
        {
            ControlHumedadTotalViejo2 medicion = new ControlHumedadTotalViejo2(CCI) { Medicion = med };
            medicion.DeleteControl = BorrarMedicion;
            listaMediciones.Children.Add(medicion);
        }

        private void NuevaMedicion_Click(object sender, RoutedEventArgs e)
        {
            ControlHumedadTotalViejo2 medicion = new ControlHumedadTotalViejo2() { Medicion = FactoriaMedicionPNT.GetDefault(IdTecnicoRecepcion, IdMuestra) };
            medicion.DeleteControl = BorrarMedicion;
            listaMediciones.Children.Add(medicion);

        }

        private void BorrarMedicion(ControlHumedadTotalViejo2 control)
        {
            listaMediciones.Children.Remove(control);
        }
    }
}
