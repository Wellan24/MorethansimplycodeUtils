using LAE.Comun.Calculos;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Biomasa.Modelo;
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

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageDensidad.xaml
    /// </summary>
    public partial class PageDensidad : UserControl
    {

        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                Prueba.Medicion = medicion;
                AddCCI(medicion);
            }
        }

        public PageDensidad()
        {
            InitializeComponent();
            Prueba.Calculo = RealizarCalculo;
            CCI.Calculo = RealizarCalculo;
        }

        private void AddCCI(MedicionPNT med)
        {
            MedicionPNT medCCI = FactoriaMedicionPNTcci.GetMedicion(med.Id);
            if (medCCI != null)
            {
                CCI.Medicion = medCCI;
                CCI.Visibility = Visibility.Visible;
                CCIAceptacion.Visibility = Visibility.Visible;
            }
            else
                CCI.Medicion = FactoriaMedicionPNT.GetDefault(Medicion.IdTecnico, Medicion.IdMuestra);
        }

        private void AddDeleteCCI_Click(object sender, RoutedEventArgs e)
        {
            if (CCI.Visibility == Visibility.Visible)
            {
                CCI.Visibility = Visibility.Collapsed;
                CCIAceptacion.Visibility = Visibility.Collapsed;
            }
            else {
                CCI.Visibility = Visibility.Visible;
                CCIAceptacion.Visibility = Visibility.Visible;
            }
        }

        private void RealizarCalculo()
        {
            Densidad densidad = new Densidad();
            if (Prueba.Densidad?.MediaDensidadHumeda != null && CCI.Densidad?.MediaDensidadHumeda != null)
            {
                densidad.IdVProcedimiento = Prueba.Densidad.IdVProcedimiento;

                Valor[] valoresHumedades = new Valor[] { Valor.Of(Prueba.Densidad.MediaDensidadHumeda, "%"), Valor.Of(CCI.Densidad.MediaDensidadHumeda, "%") };

                densidad.MediaDensidadHumeda = Calcular.Promedio(valoresHumedades).Value;
                densidad.Dif = Calcular.DiferenciaAbsolutaMaxima(valoresHumedades).Value;
                densidad.Aceptado = Calcular.EsAceptado(densidad.Dif ?? 0, densidad.IdVProcedimiento, densidad.IdParametro, densidad.MediaDensidadHumeda);

                CCIAceptacion.Densidad = densidad;
            }
            else
                CCIAceptacion.Clear();
        }

        public void RecargarHumedad()
        {
            int? nDen=Prueba.panelDensidad["IdHumedad"].SelectedIndex;
            int? nDenCCI= CCI.panelDensidad["IdHumedad"].SelectedIndex;

            Prueba.panelDensidad["IdHumedad"].InnerValues = FactoriaHumedadTotal.GetHumedades(Medicion.IdMuestra);
            CCI.panelDensidad["IdHumedad"].InnerValues = FactoriaHumedadTotal.GetHumedades(Medicion.IdMuestra);

            Prueba.panelDensidad["IdHumedad"].SelectedIndex = nDen;
            CCI.panelDensidad["IdHumedad"].SelectedIndex = nDenCCI;
        }
    }
}
