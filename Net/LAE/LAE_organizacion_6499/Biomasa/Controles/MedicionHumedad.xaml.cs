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

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para MedicionHumedad.xaml
    /// </summary>
    public partial class MedicionHumedad : UserControl
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

        public Action<MedicionHumedad> DeleteControl { get; set; }

        public MedicionHumedad()
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

        private void BorrarMedicion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas eliminar la medición? Si al finalizar la edición guarda los cambios la medición será borrada", "Borrar medición", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
                DeleteControl(this);
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
            HumedadTotal humedad = new HumedadTotal();
            if (Prueba.Humedad?.MediaHumedadTotal != null && CCI.Humedad?.MediaHumedadTotal != null)
            {
                humedad.IdVProcedimiento = Prueba.Humedad.IdVProcedimiento;

                Valor[] valoresHumedades = new Valor[] { Valor.Of(Prueba.Humedad.MediaHumedadTotal, "%"), Valor.Of(CCI.Humedad.MediaHumedadTotal, "%") };

                humedad.MediaHumedadTotal = Calcular.Promedio(valoresHumedades).Value;
                humedad.CV = Calcular.CoeficienteVariacion(valoresHumedades).Value;
                humedad.Aceptado = Calcular.EsAceptado(humedad.CV ?? 0, humedad.IdVProcedimiento, humedad.IdParametro, humedad.MediaHumedadTotal);

                CCIAceptacion.Humedad = humedad;
            }
            else
                CCIAceptacion.Clear();
        }
    }
}
