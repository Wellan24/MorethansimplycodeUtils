using LAE.Calculos;
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
    /// Lógica de interacción para PageCenizas.xaml
    /// </summary>
    public partial class PageCenizas : UserControl
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

        public PageCenizas()
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
            Cenizas cenizas = new Cenizas();
            if (Prueba.Cenizas?.MediaCenizasHU3 != null && CCI.Cenizas?.MediaCenizasHU3 != null)
            {
                cenizas.IdVProcedimiento = Prueba.Cenizas.IdVProcedimiento;

                Valor[] valoresCenizas = new Valor[] { Valor.Of(Prueba.Cenizas.MediaCenizasHU3, "%"), Valor.Of(CCI.Cenizas.MediaCenizasHU3, "%") };

                cenizas.MediaCenizasHU3 = Calcular.Promedio(valoresCenizas).Value;
                cenizas.Dif = Calcular.DiferenciaAbsolutaMaxima(valoresCenizas).Value;
                cenizas.Aceptado = Calcular.EsAceptado(cenizas.Dif ?? 0, cenizas.IdVProcedimiento, cenizas.IdParametro, cenizas.MediaCenizasHU3);

                CCIAceptacion.Cenizas = cenizas;
            }
            else
                CCIAceptacion.Clear();
        }

        public void RecargarHumedad()
        {
            int? nCen = Prueba.panelCenizas["IdHumedad3"].SelectedIndex;
            int? nCenCCI = CCI.panelCenizas["IdHumedad3"].SelectedIndex;

            Prueba.panelCenizas["IdHumedad3"].InnerValues = FactoriaHumedad3.GetHumedades(Medicion.IdMuestra);
            CCI.panelCenizas["IdHumedad3"].InnerValues = FactoriaHumedad3.GetHumedades(Medicion.IdMuestra);

            Prueba.panelCenizas["IdHumedad3"].SelectedIndex = nCen;
            CCI.panelCenizas["IdHumedad3"].SelectedIndex = nCenCCI;
        }
    }
}
